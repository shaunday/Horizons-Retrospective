using Microsoft.EntityFrameworkCore;

namespace TraJedi.Journal.Data
{
    public class TraJediDataContext : DbContext
    {
        #region Data
        public DbSet<OverallTradeModel> OverallTrades { get; set; }

        public DbSet<TradeInputModel> TradeInputs { get; set; }

        public DbSet<InputComponentModel> TradeInputComponents { get; set; }

        public DbSet<ContentModel> ContentModels { get; set; } 
        #endregion

        public TraJediDataContext(DbContextOptions<TraJediDataContext> options) : base(options)
        {

        }


    }
}
