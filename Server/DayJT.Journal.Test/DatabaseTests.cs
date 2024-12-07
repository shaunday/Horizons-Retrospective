using DayJT.Journal.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DayJT.Journal.Test
{
    [TestClass]
    public sealed class DatabaseTests
    {
        [TestMethod]
        public void CanInsertTradeComposite()
        {
            using (var context = new TradingJournalDataContext(GetDbContextOptions()))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //var author = new Author { FirstName = "a", LastName = "b" };
                //context.Authors.Add(author);
                ////Debug.WriteLine($"Before save: {author.AuthorId}");
                //context.SaveChanges();
                ////Debug.WriteLine($"After save: {author.AuthorId}");

                //Assert.AreNotEqual(0, author.AuthorId);
            }

        }



        [TestMethod]
        public void ChangeTrackerIdentifiesAddedTradeComposite()
        {
            using var context = new TradingJournalDataContext(GetDbContextOptions());

            //var author = new Author { FirstName = "a", LastName = "b" };
            //context.Authors.Add(author);
            //Assert.AreEqual(EntityState.Added, context.Entry(author).State);
        }

        private static DbContextOptions<TradingJournalDataContext> GetDbContextOptions()
        {
            var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var builder = new DbContextOptionsBuilder<TradingJournalDataContext>();
            builder.UseNpgsql(configuration.GetConnectionString("DayJTestDatabase"));
            return builder.Options;
        }
    }
}

     
    
