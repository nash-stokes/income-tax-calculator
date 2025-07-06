using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
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
builder.Services.AddScoped<TaxService>();


// Caching and ProblemDetails
builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Global error handling
app.UseExceptionHandler("/error");
app.MapGet("/error", (HttpContext ctx) =>
{
    var pd = ctx.Features.Get<ProblemDetails>()!;
    return Results.Problem(pd.Detail, statusCode: pd.Status);
});

// Tax endpoint
app.MapGet("/api/tax/{salary:decimal}", async (
    decimal salary,
    ITaxService taxService,
    CancellationToken ct) =>
{
    if (salary < 0)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["salary"] = new[] { "Salary must be non-negative." }
        });

    var result = await taxService.ComputeAsync(salary, ct);
    return Results.Ok(result);
});

app.Run();