using API.Data;
using API.Data.Repositories;
using API.Services;
using API.Services.TaxCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load secrets from env var or user-secrets
builder.Configuration.AddEnvironmentVariables();

// DbContext and Repository
builder.Services.AddDbContext<TaxDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("TaxDb")));
builder.Services.AddScoped<ITaxBandRepository, TaxBandRepository>();
builder.Services.AddScoped<ITaxCalculatorFactory, TaxCalculatorFactory>();
builder.Services.AddScoped<ITaxCalculator>(sp =>
{
    var factory = sp.GetRequiredService<ITaxCalculatorFactory>();
    return factory.CreateCalculatorAsync(CancellationToken.None).GetAwaiter().GetResult();
});
builder.Services.AddScoped<ITaxService, TaxService>();


// Caching and ProblemDetails
builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Global error handling
app.UseExceptionHandler("/error");
app.MapGet("/error", (HttpContext ctx) =>
{
    var pd = ctx.Features.Get<ProblemDetails>()!;
    return Results.Problem(pd.Detail, statusCode: pd.Status);
});

app.MapControllers();


app.Run();