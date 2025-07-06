using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        /// <summary>
        /// Gets the tax calculation for a given salary
        /// </summary>
        /// <param name="salary">The annual salary (must be non-negative)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Tax calculation result</returns>
        /// <response code="400">If salary is negative</response>
        /// <response code="200">Returns the tax calculation</response>
        [HttpGet("{salary:decimal}")]
        public async Task<IActionResult> GetTax(decimal salary, CancellationToken ct)
        {
            if (salary < 0)
            {
                return BadRequest("Salary must be non-negative.");
            }

            var result = await _taxService.ComputeAsync(salary, ct);
            return Ok(result);
        }
    }
}