using HotelReservation.Application.Contracts;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
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
            var roles = await _rolService.GetAllAsync();
            return Ok(roles);
        }

        [HttpPost("InsertRole")]
        public async Task<IActionResult> Create([FromBody] Rol rol)
        {
            await _rolService.AddAsync(rol);
            return Ok(new { message = "Rol registrado correctamente" });
        }
    }
}
