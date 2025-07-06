namespace HsR.Web.API.Settings
{
    public class CacheSettings
    {
        public int CacheDurationMinutes { get; set; } = 5;
        public int PreloadPageCount { get; set; } = 3;
        public int SizeLimit { get; set; } = 1024;
        public int MaxConcurrentUsers { get; set; } = 1000;
        public int DefaultPageSize { get; set; } = 20;
        public int LoadWaitTimeoutSeconds { get; set; } = 2;
        public int CleanupInactiveUsersThresholdHours { get; set; } = 2;
        public int CleanupIntervalMinutes { get; set; } = 30;
    }
} 