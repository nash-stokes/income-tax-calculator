using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages;

public class IndexModel : PageModel
{
    private readonly ITaxService _taxService;

    public IndexModel(ITaxService taxService)
    {
        _taxService = taxService;
    }

    [BindProperty] public decimal GrossSalary { get; set; }

    public TaxResult? Result { get; private set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (GrossSalary < 0)
        {
            ModelState.AddModelError(nameof(GrossSalary),
                "Must be non-negative");
            return Page();
        }

        Result = await _taxService.ComputeAsync(
            GrossSalary, HttpContext.RequestAborted);
        return Page();
    }
}