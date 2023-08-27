using IdentityProvider.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProvider.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>() { new Claim("UserId", user.Id), new Claim(ClaimTypes.Name, user.UserName) };
            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private SymmetricSecurityKey GetSecurityKey()
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
    }
}
