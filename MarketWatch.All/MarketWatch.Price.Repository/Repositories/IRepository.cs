using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Price.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();
    }
}
