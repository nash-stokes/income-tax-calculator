using API.Models.DTOs;

namespace API.Services;

public interface ITaxService
{
    Task<TaxResult> ComputeAsync(decimal grossAnnual, CancellationToken ct);
}