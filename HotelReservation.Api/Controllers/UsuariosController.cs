using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            var usuariosDto = _mapper.Map<IEnumerable<ObtenerUsuarioDto>>(usuarios);
            return Ok(usuariosDto);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(_mapper.Map<ObtenerUsuarioDto>(usuario));
        }

        [HttpPost("InsertUser")]
        public async Task<IActionResult> Create([FromBody] InsertarUsuarioDto dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);
            await _usuarioService.AddAsync(_mapper.Map<InsertarUsuarioDto>(usuario));
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarUsuarioDto dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);
            usuario.IdUsuario = id;
            await _usuarioService.UpdateAsync(_mapper.Map<ActualizarUsuarioDto>(usuario));
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
