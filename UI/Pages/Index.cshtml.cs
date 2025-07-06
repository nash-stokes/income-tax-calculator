using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using API.Models.DTOs;

namespace UI.Pages;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TaxApi");
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

        try 
        {
            var response = await _httpClient.GetFromJsonAsync<TaxResult>(
                $"api/tax/{GrossSalary}", 
                HttpContext.RequestAborted);
            
            Result = response;
            return Page();
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, 
                "Unable to connect to the tax calculation service. Please try again later.");
            return Page();
        }
        catch (JsonException ex)
        {
            ModelState.AddModelError(string.Empty, 
                "Invalid response from the tax calculation service.");
            return Page();
        }
    }

}