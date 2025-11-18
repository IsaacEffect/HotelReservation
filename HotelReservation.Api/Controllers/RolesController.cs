using HotelReservation.Api.Extensions;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rolService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _rolService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("InsertRole")]
        public async Task<IActionResult> Create([FromBody] InsertarRolDto dto)
        {
            var result = await _rolService.AddAsync(dto);
            return result.ToActionResult();
        }
    }
}
