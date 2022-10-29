using Microsoft.EntityFrameworkCore;
using TraJedi.Journal.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TraJediDataContext>(
    o=> o.UseNpgsql(builder.Configuration.GetConnectionString("traJediDatabase"))
    );

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//todo setup rest here

app.Run();

