using DayJT.Journal.DataContext.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;


namespace PubAppTest;

[TestClass]
public class InMemoryTests
{
    [TestMethod]
    public void CanInsertCompositeIntoDatabase()
    {
        var builder = new DbContextOptionsBuilder<TradingJournalDataContext>();
        //builder.UseSqlServer(
        //    "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubTestData");
        var _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        builder.UseSqlite(_connection);
        using (var context = new TradingJournalDataContext(builder.Options))
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //var author = new Author { FirstName = "a", LastName = "b" };
            //context.Authors.Add(author);
            //Debug.WriteLine($"Before save: {author.AuthorId}");
            context.SaveChanges();
            //Debug.WriteLine($"After save: {author.AuthorId}");
            //Assert.AreNotEqual(0, author.AuthorId);
        }
    }

    [TestMethod]
    public void ChangeTrackerIdentifiesAddedComposite()
    {
        var builder = new DbContextOptionsBuilder<TradingJournalDataContext>().UseSqlite("Filename=:memory:");
        using var context = new TradingJournalDataContext(builder.Options);
        //var author = new Author { FirstName = "a", LastName = "b" };
        //context.Authors.Add(author);
        //Assert.AreEqual(EntityState.Added, context.Entry(author).State);
    }

    //[TestMethod]
    //public void InsertCompositesReturnsCorrectResultNumber()
    //{
    //    using TradingJournalDataContext context = SetUpSQLiteMemoryContextWithOpenConnection();
    //    var authorList = new Dictionary<string, string>
    //            { { "a" , "b" },
    //              { "c" , "d" },
    //              { "d" , "e" }
    //            };

    //    var dl = new DataLogic(context);
    //    Assert.AreEqual(authorList.Count, dl.ImportAuthors(authorList));
    //}

    private static TradingJournalDataContext SetUpSQLiteMemoryContextWithOpenConnection()
    {
        var builder = new DbContextOptionsBuilder<TradingJournalDataContext>().UseSqlite("Filename=:memory:");
        var context = new TradingJournalDataContext(builder.Options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }
}

