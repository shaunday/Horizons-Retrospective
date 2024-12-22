using JTA.Journal.Repository;
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
        var _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        builder.UseSqlite(_connection);
        using (var context = new TradingJournalDataContext(builder.Options))
        {
            EnsureCreated(context);
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
        using var context = GetSqliteContext();
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

    private static TradingJournalDataContext GetSqliteContext()
    {
        var builder = new DbContextOptionsBuilder<TradingJournalDataContext>().UseSqlite("Filename=:memory:");
        var context = new TradingJournalDataContext(builder.Options);
        return context;
    }

    private static void EnsureCreated(TradingJournalDataContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

}

