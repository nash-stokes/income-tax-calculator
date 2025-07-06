namespace API.Models.DTOs;

public record TaxResult(
    decimal GrossAnnual,
    decimal GrossMonthly,
    decimal AnnualTax,
    decimal MonthlyTax,
    decimal NetAnnual,
    decimal NetMonthly);