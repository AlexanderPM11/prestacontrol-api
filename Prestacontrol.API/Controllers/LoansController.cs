using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prestacontrol.Application.DTOs;
using Prestacontrol.Application.Interfaces;
using System.Security.Claims;

namespace Prestacontrol.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoansController(ILoanService loanService) => _loanService = loanService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _loanService.CreateLoanAsync(request, userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _loanService.GetClientLoansAsync(""); // Empty string means all
            return Ok(result);
        }

        [HttpGet("client/{clientName}")]
        public async Task<IActionResult> GetByClient(string clientName)
        {
            var result = await _loanService.GetClientLoansAsync(clientName);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var result = await _loanService.GetLoanDetailsAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
