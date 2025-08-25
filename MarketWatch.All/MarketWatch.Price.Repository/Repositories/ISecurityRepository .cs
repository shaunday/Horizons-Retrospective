using MarketWatch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Price.Repository
{
    public interface ISecurityRepository : IRepository<SecurityModel>
    {
        Task<SecurityModel?> GetBySymbolAsync(string symbol);
    }
}
