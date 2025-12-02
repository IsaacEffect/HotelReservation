using HotelReservation.Api.Extensions;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _clienteService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("GetClientById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _clienteService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("InsertClient")]
        public async Task<IActionResult> Create([FromBody] InsertarClienteDto dto)
        {
            var result = await _clienteService.AddAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("ModifyClient/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarClienteDto dto)
        {
            var result = await _clienteService.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("DeleteClient/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _clienteService.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}
