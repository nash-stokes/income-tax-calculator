using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public interface ITaxBandRepository
{
    Task<IReadOnlyList<TaxBand>> GetAllAsync(CancellationToken ct);
}

public class TaxBandRepository(TaxDbContext ctx) : ITaxBandRepository
{
    public async Task<IReadOnlyList<TaxBand>> GetAllAsync(CancellationToken ct)
    {
        return await ctx.TaxBands
            .AsNoTracking()
            .OrderBy(tb => tb.LowerLimit)
            .ToListAsync(ct);
    }
}