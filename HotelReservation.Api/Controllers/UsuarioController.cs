using HotelReservation.Application.Contracts;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        [HttpPost("InsertUser")]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            await _usuarioService.AddAsync(usuario);
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Usuario usuario)
        {
            usuario.IdUsuario = id;
            await _usuarioService.UpdateAsync(usuario);
            return Ok(new { message = "Usuario modificado correctamente" });
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _usuarioService.DeleteAsync(id);
            return Ok(new { message = "Usuario eliminado correctamente" });
        }
    }
}
