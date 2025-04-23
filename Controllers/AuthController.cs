using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NutriCheck.Data;
using NutriCheck.Models;
using NutriCheck.Settings;
using Microsoft.Extensions.Options;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MongoDbService _db;
        private readonly JwtSettings  _jwt;

        public AuthController(MongoDbService db, IOptions<JwtSettings> jwtOpts)
        {
            _db  = db;
            _jwt = jwtOpts.Value;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] Nutricionista cred)
        {
            var user = _db.Nutricionistas
                         .Find(n => n.Email == cred.Email && n.Password == cred.Password)
                         .FirstOrDefault();
            if (user == null) 
                return Unauthorized("Credenciales inválidas");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwt.ExpiryHours),
                signingCredentials: creds);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
