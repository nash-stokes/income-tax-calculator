using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<TaxDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration["TAX_DB_CONN"]));
builder.Services.AddScoped<ITaxBandRepository, TaxBandRepository>();

// Strategy & Service
builder.Services.AddSingleton<ITaxCalculator, BandBasedTaxCalculator>();
builder.Services.AddScoped<ITaxService, TaxService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();