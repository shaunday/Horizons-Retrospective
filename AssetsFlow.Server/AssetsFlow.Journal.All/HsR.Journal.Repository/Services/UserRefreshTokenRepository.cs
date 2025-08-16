using HsR.Common.AspNet.Authentication;
using HsR.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class UserRefreshTokenRepository : IUserRefreshTokenRepository
{
    private readonly TradingJournalDataContext _dbContext;

    public UserRefreshTokenRepository(TradingJournalDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiry)
    {
        var user = await _dbContext.UserData.FindAsync(userId);
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<(string? Token, DateTime? Expiry)> GetRefreshTokenAsync(Guid userId)
    {
        var user = await _dbContext.UserData.FindAsync(userId);
        return (user?.RefreshToken, user?.RefreshTokenExpiry);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        var user = await _dbContext.UserData.FindAsync(userId);
        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _dbContext.SaveChangesAsync();
        }
    }
}
