using MarketWatch.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Price.Repository
{
    public class SecurityRepository : Repository<SecurityModel>, ISecurityRepository
    {
        public SecurityRepository(MarketDbContext context) : base(context)
        {
        }

        public async Task<SecurityModel?> GetBySymbolAsync(string symbol)
        {
            return await _dbSet
                .Include(s => s.PriceBars)
                .FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}
