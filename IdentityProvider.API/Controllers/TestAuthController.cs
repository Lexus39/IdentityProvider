using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityProvider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("Test")]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            var name = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return Ok(name);
        }
    }
}
