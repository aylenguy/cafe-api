using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CafeApi.Models;

namespace CafeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>Login del administrador — devuelve JWT</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDto), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioValido = _config["Admin:Usuario"];
            var passwordValida = _config["Admin:Password"];

            // Comparación en tiempo constante para evitar timing attacks
            var usuarioCorrecto = string.Equals(request.Usuario, usuarioValido, StringComparison.Ordinal);
            var passwordCorrecta = string.Equals(request.Password, passwordValida, StringComparison.Ordinal);

            if (!usuarioCorrecto || !passwordCorrecta)
            {
                _logger.LogWarning("Intento de login fallido para usuario: {Usuario}", request.Usuario);
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            var token = GenerarToken();
            _logger.LogInformation("Login exitoso para: {Usuario}", request.Usuario);

            return Ok(new TokenResponseDto
            {
                Token = token,
                Expira = DateTime.UtcNow.AddHours(8),
                TipoToken = "Bearer"
            });
        }

        private string GenerarToken()
        {
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key no configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role,  "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}