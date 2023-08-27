using IdentityProvider.API.Models;
using IdentityProvider.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Results.Json(ModelState));
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Login exception");
                return BadRequest(Results.Json(ModelState));
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var jwtToken = _jwtService.CreateToken(user);
                return Ok(Results.Json(jwtToken));
            }
            ModelState.AddModelError(string.Empty, "Login exception");
            return BadRequest(Results.Json(ModelState));
        }

        [HttpPost("Sign-up")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Results.Json(ModelState));
            }
            if (model.Password != model.ConfirmedPassword)
            {
                ModelState.AddModelError(string.Empty, "Confirm password error");
                return BadRequest(Results.Json(ModelState));
            }
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError(string.Empty, "Username error");
                return BadRequest(Results.Json(ModelState));
            }
            var result = await _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = model.UserName
            }, model.Password);
            if (result.Succeeded)
            {
                return Created("", model);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Description, "Sign up error");
            }
            return BadRequest(Results.Json(ModelState));
        }
    }
}
