using MarketWatch.Entities;
using MarketWatch.Price.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketWatch.Data.Services
{
    public interface ISecurityService
    {
        Task<SecurityModel> GetBySymbolAsync(string symbol);
        Task AddSecurityAsync(SecurityModel security);
        Task AddPriceBarsAsync(SecurityModel security, IEnumerable<PriceBarModel> bars);
        Task SaveChangesAsync();
    }

    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;

        public SecurityService(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public async Task<SecurityModel> GetBySymbolAsync(string symbol)
        {
            return await _securityRepository.GetBySymbolAsync(symbol);
        }

        public async Task AddSecurityAsync(SecurityModel security)
        {
            await _securityRepository.AddAsync(security);
        }

        public async Task AddPriceBarsAsync(SecurityModel security, IEnumerable<PriceBarModel> bars)
        {
            foreach (var bar in bars)
            {
                bar.SecurityFK = security.Id;
                security.PriceBars.Add(bar);
            }
            await _securityRepository.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _securityRepository.SaveChangesAsync();
        }
    }
}
