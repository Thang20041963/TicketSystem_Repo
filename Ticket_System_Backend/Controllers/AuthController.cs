using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Ticket_System_Backend.DTOs;
using Ticket_System_Backend.Services;

namespace Ticket_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
           
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest model)
        {
            if (ModelState.IsValid)
            {
              var response = await _authService.LoginAsync(model);
              return Ok(response);

            }
            return Unauthorized();
        }

    }
}
