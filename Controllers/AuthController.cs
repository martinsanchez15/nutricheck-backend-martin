using Microsoft.AspNetCore.Mvc;
using NutriCheck.Models;
using NutriCheck.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NutriCheck.Controllers
{
    /// <summary>
    /// Controlador de autenticación para gestionar el inicio de sesión y la generación de tokens JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <param name="config">Configuración de la aplicación.</param>
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Endpoint para iniciar sesión y obtener un token JWT.
        /// </summary>
        /// <param name="login">Objeto que contiene el email y la contraseña del nutricionista.</param>
        /// <returns>Devuelve un token JWT junto con la información del usuario.</returns>
        /// <response code="200">Si las credenciales son correctas y el token se genera correctamente.</response>
        /// <response code="401">Si las credenciales son incorrectas.</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] Nutricionista login)
        {
            var usuario = _context.Nutricionistas
                .FirstOrDefault(n => n.Email == login.Email && n.Password == login.Password);

            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos");
            }

            var token = GenerarToken(usuario);

            return Ok(new
            {
                token,
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.Rol
            });
        }

        /// <summary>
        /// Método para generar un token JWT a partir de los datos del usuario.
        /// </summary>
        /// <param name="usuario">El usuario que está intentando autenticarse.</param>
        /// <returns>Un token JWT como cadena de texto.</returns>
        private string GenerarToken(Nutricionista usuario)
        {
            // Generar una clave de 256 bits (32 bytes) de forma segura
            var key = Encoding.UTF8.GetBytes("nutricheck-superclave-segura-2025!!");



            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Nutricionista")
            };

            var credenciales = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Método para generar una clave de 256 bits (32 bytes) de forma segura usando RandomNumberGenerator.
        /// </summary>
        /// <returns>Un arreglo de bytes que representa la clave segura.</returns>
        private byte[] GenerarClaveSegura()
        {
            // Usamos RandomNumberGenerator para generar la clave
            return RandomNumberGenerator.GetBytes(32); // 32 bytes = 256 bits
        }
    }
}
