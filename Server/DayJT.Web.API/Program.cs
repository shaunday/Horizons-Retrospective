using AutoMapper;
using AutoMapper.EntityFramework;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using DayJT.Journal.Data;
using DayJT.Journal.Data.Services;
using DayJT.Web.API.Mapping;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("/logs/traJedi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    //input or output formatters
    options.ReturnHttpNotAcceptable = true; //default is json - won't accept requests for diff formats
});

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TradingJournalDataContext>
                (options => options.UseNpgsql(builder.Configuration.GetConnectionString("DayJTradingDbKey")), ServiceLifetime.Singleton);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(setupAction =>
//{
//    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

//    setupAction.IncludeXmlComments(xmlCommentsFullPath);
//});


// Auto Mapper 
Action<IMapperConfigurationExpression> configAction = (mce) =>
{
    mce.AddMaps(Assembly.GetExecutingAssembly()); // all profiles
    mce.AddCollectionMappers();
};

builder.Services.AddAutoMapper(configAction); 


//builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddScoped<IJournalRepository, JournalRepository>();

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    app.MapControllers();
});

app.Run();
