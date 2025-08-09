using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace HsR.Common.AspNet.Authentication
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        Task ReplaceRefreshTokenAsync(Guid userId, string oldToken, string newToken);
        Task RevokeRefreshTokenAsync(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRefreshTokenRepository _userTokenRepo;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<RefreshTokenService> _logger;

        public RefreshTokenService(
            IUserRefreshTokenRepository userTokenRepo,
            IOptions<JwtSettings> jwtSettings,
            ILogger<RefreshTokenService> logger)
        {
            _userTokenRepo = userTokenRepo;
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

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var expiry = DateTime.UtcNow.AddDays(7);
            await _userTokenRepo.SaveRefreshTokenAsync(userId, refreshToken, expiry);
            _logger.LogInformation("Saved refresh token for user {UserId} expiring at {Expiry}", userId, expiry);
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var (token, expiry) = await _userTokenRepo.GetRefreshTokenAsync(userId);
            return token == refreshToken && expiry > DateTime.UtcNow;
        }

        public async Task ReplaceRefreshTokenAsync(Guid userId, string oldToken, string newToken)
        {
            var (token, _) = await _userTokenRepo.GetRefreshTokenAsync(userId);
            if (token == oldToken)
            {
                var newExpiry = DateTime.UtcNow.AddDays(7);
                await _userTokenRepo.SaveRefreshTokenAsync(userId, newToken, newExpiry);
                _logger.LogInformation("Replaced refresh token for user {UserId}", userId);
            }
        }

        public Task RevokeRefreshTokenAsync(Guid userId)
        {
            return _userTokenRepo.RevokeRefreshTokenAsync(userId);
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
                    ValidateLifetime = false // allow expired tokens
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
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
