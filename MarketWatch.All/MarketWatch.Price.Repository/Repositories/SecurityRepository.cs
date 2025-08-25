using MarketWatch.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Price.Repository
{
    public interface ISecurityRepository : IRepository<SecurityModel>
    {
        Task<SecurityModel?> GetBySymbolAsync(string symbol, Timeframe? timeframe = null);
    }

    public class SecurityRepository : Repository<SecurityModel>, ISecurityRepository
    {
        public SecurityRepository(MarketDbContext context) : base(context)
        {
        }

        public async Task<SecurityModel?> GetBySymbolAsync(string symbol, Timeframe? timeframe = null)
        {
            var query = _dbSet.AsQueryable();

            query = query.Where(s => s.Symbol == symbol);

            if (timeframe.HasValue)
            {
                query = query.Include(s => s.PriceBars.Where(pb => pb.Timeframe == timeframe.Value));
            }
            else
            {
                query = query.Include(s => s.PriceBars);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
