namespace HsR.Common.AspNet.Authentication
{
    public interface IUserRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry);
        Task<(string? Token, DateTime? Expiry)> GetRefreshTokenAsync(Guid userId);
        Task RevokeRefreshTokenAsync(Guid userId);
    }

}
