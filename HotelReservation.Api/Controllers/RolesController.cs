using AutoMapper;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administrador")]
    [Authorize(Roles = "Empleado")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;
        private readonly IMapper _mapper;

        public RolesController(IRolService rolService, IMapper mapper)
        {
            _rolService = rolService;
            _mapper = mapper;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolService.GetAllAsync();
            var rolesDto = _mapper.Map<IEnumerable<ObtenerRolDto>>(roles);
            return Ok(rolesDto);
        }

        [HttpPost("InsertRole")]
        public async Task<IActionResult> Create([FromBody] InsertarRolDto dto)
        {
            var rol = _mapper.Map<Rol>(dto);
            await _rolService.AddAsync(_mapper.Map<InsertarRolDto>(rol));
            return Ok(new { message = "Rol registrado correctamente" });
        }
    }
}
