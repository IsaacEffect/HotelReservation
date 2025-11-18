using HotelReservation.Api.Configurations;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelReservation.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUsuarioService usuarioService, JwtSettings jwtSettings, ILogger<AuthController> logger)
        {
            _usuarioService = usuarioService;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Intento de login con modelo inválido.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Intento de login para el usuario {Correo}", dto.Correo);

                var result = await _usuarioService.ValidarCredencialesAsync(dto.Correo, dto.Contrasena);

                if (!result.Success)
                {
                    _logger.LogWarning("Login fallido para {Correo}: {Mensaje}", dto.Correo, result.Message);
                    return Unauthorized(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                if (result.Data == null)
                {
                    _logger.LogError("Error generando token: el usuarioDto está vacío para {Correo}", dto.Correo);
                    return BadRequest(new
                    {
                        success = false,
                        message = "No se pudo generar el token, usuario inválido."
                    });
                }

                var token = GenerarToken(result.Data);

                _logger.LogInformation("Usuario {Correo} inició sesión correctamente.", dto.Correo);

                return Ok(new
                {
                    success = true,
                    message = "Login exitoso",
                    token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de login para {Correo}", dto.Correo);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor durante el login."
                });
            }
        }

        private string GenerarToken(ObtenerUsuarioDto usuarioDto)
        {
            _logger.LogInformation("Generando token para usuario ID {IdUsuario}", usuarioDto.IdUsuario);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuarioDto.Correo),
                new Claim(ClaimTypes.Role, usuarioDto.Rol?.NombreRol ?? "Empleado")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Token generado exitosamente para usuario ID {IdUsuario}", usuarioDto.IdUsuario);

            return jwt;
        }
    }
}
