using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using TraJedi.Journal.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //input or output formatters
    options.ReturnHttpNotAcceptable = true; //default is json - won't accept requests for diff formats
});

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TradingJournalDataContext>
                (options => options.UseNpgsql(builder.Configuration.GetConnectionString("traJediDatabase")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddSingleton<TradingJournalAccess>();

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
