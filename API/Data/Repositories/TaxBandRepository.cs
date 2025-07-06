using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

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