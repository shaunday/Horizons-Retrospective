using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HsR.Common.AspNet.Authentication
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
        Task ReplaceRefreshTokenAsync(string userId, string oldToken, string newToken);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }

public class RefreshTokenService : IRefreshTokenService
    {
        // Simulated storage: userId -> (refreshToken, expiry)
        private readonly ConcurrentDictionary<string, (string Token, DateTime Expiry)> _refreshTokens = new();
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<RefreshTokenService> _logger;

        public RefreshTokenService(IOptions<JwtSettings> jwtSettings, ILogger<RefreshTokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var expiry = DateTime.UtcNow.AddDays(7); // Refresh token valid 7 days
            _refreshTokens[userId] = (refreshToken, expiry);
            _logger.LogInformation("Saved refresh token for user {UserId} expiring at {Expiry}", userId, expiry);
            return Task.CompletedTask;
        }

        public Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            if (_refreshTokens.TryGetValue(userId, out var storedToken))
            {
                var isValid = storedToken.Token == refreshToken && storedToken.Expiry > DateTime.UtcNow;
                return Task.FromResult(isValid);
            }
            return Task.FromResult(false);
        }

        public Task ReplaceRefreshTokenAsync(string userId, string oldToken, string newToken)
        {
            if (_refreshTokens.TryGetValue(userId, out var storedToken) && storedToken.Token == oldToken)
            {
                var expiry = DateTime.UtcNow.AddDays(7);
                _refreshTokens[userId] = (newToken, expiry);
                _logger.LogInformation("Replaced refresh token for user {UserId}", userId);
            }
            return Task.CompletedTask;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey)),
                    ValidateLifetime = false // Here we want to get principal even if token expired
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get principal from expired token");
                return null;
            }
        }
    }

}
