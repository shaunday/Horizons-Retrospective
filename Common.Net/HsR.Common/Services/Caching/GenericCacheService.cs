using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HsR.Common.Services.Caching
{
    public interface ICacheService<TId, TModel>
    {
        Task<TModel?> GetAsync(TId id, string? subKey = null, Func<TId, string?, TModel?>? fallback = null, TimeSpan? waitTimeout = null);
        void Set(TId id, TModel value, string? subKey = null, TimeSpan? duration = null);
        void Invalidate(TId id, string? subKey = null);
        void InvalidateAndReload(TId id, string? subKey = null);
        void CleanupInactive(TimeSpan inactivityThreshold);
    }

    public abstract class CacheServiceBase<TId, TModel> : ICacheService<TId, TModel> where TId : notnull
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        private readonly TimeSpan _defaultDuration;
        private readonly int _maxConcurrentUsers;

        private readonly ConcurrentDictionary<TId, Task> _loadTasks = new();
        private readonly ConcurrentDictionary<TId, CancellationTokenSource> _loadCts = new();
        private readonly ConcurrentDictionary<TId, CancellationTokenSource> _userEvictionTokens = new();
        private readonly ConcurrentDictionary<TId, DateTime> _lastAccess = new();
        private readonly ConcurrentDictionary<TId, SemaphoreSlim> _userSemaphores = new();

        protected abstract string CacheKeyPrefix { get; }
        protected abstract Task<TModel> LoadFromSourceAsync(TId id, CancellationToken token);

        protected CacheServiceBase(
            IMemoryCache memoryCache,
            ILogger logger,
            TimeSpan defaultDuration,
            int maxConcurrentUsers)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _defaultDuration = defaultDuration;
            _maxConcurrentUsers = maxConcurrentUsers;
        }

        protected virtual string GetCacheKey(TId id, string? subKey = null)
            => $"{CacheKeyPrefix}{id}{(subKey != null ? $"_{subKey}" : string.Empty)}";

        private CancellationTokenSource GetOrCreateEvictionToken(TId id)
            => _userEvictionTokens.GetOrAdd(id, _ => new CancellationTokenSource());

        private SemaphoreSlim GetUserSemaphore(TId id)
            => _userSemaphores.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));

        private void UpdateLastAccess(TId id)
            => _lastAccess[id] = DateTime.UtcNow;

        private void EnforceLimits()
        {
            int currentUsers = new[] { _loadTasks.Count, _loadCts.Count, _userEvictionTokens.Count, _lastAccess.Count, _userSemaphores.Count }.Max();
            if (currentUsers >= _maxConcurrentUsers)
            {
                _logger.LogDebug("MaxConcurrentUsers reached ({Current} >= {Max}), cleaning inactive users...", currentUsers, _maxConcurrentUsers);
                CleanupInactive(TimeSpan.FromHours(1));
            }
        }

        public async Task<TModel?> GetAsync(TId id, string? subKey = null, Func<TId, string?, TModel?>? fallback = null, TimeSpan? waitTimeout = null)
        {
            EnforceLimits();

            string key = GetCacheKey(id, subKey);
            if (_memoryCache.TryGetValue<TModel>(key, out var cached))
            {
                UpdateLastAccess(id);
                return cached;
            }

            if (_loadTasks.TryGetValue(id, out var task) && !task.IsCompleted)
            {
                try
                {
                    using var cts = new CancellationTokenSource(waitTimeout ?? TimeSpan.FromSeconds(10));
                    await task.WaitAsync(cts.Token);

                    if (_memoryCache.TryGetValue<TModel>(key, out cached))
                    {
                        UpdateLastAccess(id);
                        return cached;
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogDebug("Cache load wait timed out for {Id} (subKey: {SubKey})", id, subKey);
                }
            }

            return fallback != null ? fallback(id, subKey) : default;
        }

        public void Set(TId id, TModel value, string? subKey = null, TimeSpan? duration = null)
        {
            EnforceLimits();
            UpdateLastAccess(id);

            string key = GetCacheKey(id, subKey);
            var options = CreateCacheEntryOptions(id);
            _memoryCache.Set(key, value, options);
        }

        public void Invalidate(TId id, string? subKey = null)
        {
            string key = GetCacheKey(id, subKey);
            _memoryCache.Remove(key);
        }

        public void InvalidateAndReload(TId id, string? subKey = null)
        {
            EnforceLimits();
            CancelExisting(id);

            // Ensure a new eviction token exists immediately
            _userEvictionTokens[id] = new CancellationTokenSource();

            var cts = new CancellationTokenSource();
            _loadCts[id] = cts;
            _loadTasks[id] = Task.Run(() => LoadCacheAsync(id, subKey, cts.Token));
        }

        protected virtual async Task LoadCacheAsync(TId id, string? subKey, CancellationToken token)
        {
            var sem = GetUserSemaphore(id);
            await sem.WaitAsync(token);
            try
            {
                var value = await LoadFromSourceAsync(id, token);
                if (value != null)
                    Set(id, value, subKey);
            }
            finally
            {
                sem.Release();
            }
        }

        private void CancelExisting(TId id)
        {
            if (_loadCts.TryRemove(id, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public void CleanupInactive(TimeSpan inactivityThreshold)
        {
            var cutoff = DateTime.UtcNow - inactivityThreshold;

            foreach (var kvp in _lastAccess)
            {
                if (kvp.Value < cutoff)
                {
                    var userId = kvp.Key;

                    if (_userEvictionTokens.TryRemove(userId, out var evictCts))
                    {
                        evictCts.Cancel();
                        evictCts.Dispose();
                    }

                    if (_userSemaphores.TryRemove(userId, out var sem))
                        sem.Dispose();

                    if (_loadCts.TryRemove(userId, out var loadCts))
                    {
                        loadCts.Cancel();
                        loadCts.Dispose();
                    }

                    _loadTasks.TryRemove(userId, out _);
                    _lastAccess.TryRemove(userId, out _);

                    _logger.LogDebug("Cleaned up inactive cache user {UserId}", userId);
                }
            }

            var completed = _loadTasks.Where(kvp => kvp.Value.IsCompleted).ToList();
            foreach (var kvp in completed)
                _loadTasks.TryRemove(kvp.Key, out _);
        }

        #region Helpers

        private MemoryCacheEntryOptions CreateCacheEntryOptions(TId id)
        {
            var token = GetOrCreateEvictionToken(id);

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_defaultDuration)
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1);

            options.ExpirationTokens.Add(new CancellationChangeToken(token.Token));
            options.RegisterPostEvictionCallback((k, v, r, s) =>
                _logger.LogDebug("Cache entry {Key} evicted due to {Reason}", k, r));

            return options;
        }

        #endregion
    }
}
