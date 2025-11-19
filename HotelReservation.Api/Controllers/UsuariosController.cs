using HotelReservation.Api.Extensions;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservation.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
>>>>>>> origin/develop
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API - GetAllUsers llamado.");
            var result = await _usuarioService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("API - GetUserById llamado para ID {Id}", id);
            var result = await _usuarioService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("InsertUser")]
        public async Task<IActionResult> Create([FromBody] InsertarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("API - InsertUser llamado para {Correo}", dto.Correo);
            var result = await _usuarioService.AddAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("API - UpdateUser llamado para ID {Id}", id);
            var result = await _usuarioService.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

<<<<<<< HEAD
=======
        [Authorize(Roles = "Administrador,Empleado")]
>>>>>>> origin/develop
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

<<<<<<< HEAD
=======
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Si es empleado, NO puede cambiar la de otro
            if (role == "Empleado" && userIdToken != dto.IdUsuario.ToString())
                return Forbid();

>>>>>>> origin/develop
            _logger.LogInformation("API - ChangePassword llamado para ID {IdUsuario}", dto.IdUsuario);
            var result = await _usuarioService.CambiarContrasenaAsync(dto);
            return result.ToActionResult();
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("API - DeleteUser llamado para ID {Id}", id);
            var result = await _usuarioService.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}
