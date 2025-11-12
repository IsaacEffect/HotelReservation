using HotelReservation.Api.Configurations;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
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

        public AuthController(IUsuarioService usuarioService, JwtSettings jwtSettings)
        {
            _usuarioService = usuarioService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _usuarioService.ValidarCredencialesAsync(dto.Correo, dto.Contrasena);
            if (!result.Success)
                return Unauthorized(result.Message);

            if (result.Data == null)
                return BadRequest("No se pudo generar el token, usuarioDto inválido.");

            var token = GenerarToken(result.Data);


            return Ok(new
            {
                success = true,
                message = "Login exitoso",
                token
            });
        }

        private string GenerarToken(ObtenerUsuarioDto usuarioDto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDto.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuarioDto.Correo),
                new Claim(ClaimTypes.Role, usuarioDto.Rol?.NombreRol ?? "Cliente")

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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
