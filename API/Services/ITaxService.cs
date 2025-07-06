using Application.DTOs;

namespace Application.Interfaces;

public interface ITaxService
{
    Task<TaxResult> ComputeAsync(decimal grossAnnual, CancellationToken ct);
}