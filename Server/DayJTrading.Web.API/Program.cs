using Asp.Versioning;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DayJT.Journal.DataContext.Services;
using DayJT.Journal.Repository;
using DayJT.Journal.Repository.Services;
using DayJT.Web.API.Mapping;
using DayJTrading.Journal.Seeder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("/logs/traJedi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    //input or output formatters
    options.ReturnHttpNotAcceptable = true; //default is json - won't accept requests for diff formats
    options.OutputFormatters.Insert(0, new SystemTextJsonOutputFormatter(new JsonSerializerOptions
    {
        WriteIndented = true
    }));
});

// Add appsettings.json to the configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<TradingJournalDataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DayJTradingDbKey"));

    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging(); // Enable logging in Development
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});


// Auto Mapper 
Action<IMapperConfigurationExpression> configAction = (mce) =>
{
    mce.AddMaps(typeof(JournalObjectsMappingProfile).Assembly); // all profiles
    mce.AddCollectionMappers();
};
builder.Services.AddAutoMapper(configAction); 


//builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddScoped<IJournalRepository, JournalRepository>();

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

var app = builder.Build();

// seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TradingJournalDataContext>();

    var seeder = new DatabaseSeeder(dbContext);
    await seeder.SeedAsync();  // Seed data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // General developer exception page

    // Use a custom middleware to handle database exceptions
    app.Use(async (context, next) =>
    {
        try
        {
            await next.Invoke();
        }
        catch (Exception ex) when (ex is DbUpdateException || ex is SqlException)
        {
            Log.Logger.Error("Db exception - " + ex);
        }
    });
}

app.Run();
