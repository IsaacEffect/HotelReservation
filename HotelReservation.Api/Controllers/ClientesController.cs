using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
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
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("GetClientById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            return cliente is null ? NotFound() : Ok(cliente);
        }

        [HttpPost("InsertClient")]
        public async Task<IActionResult> Create([FromBody] InsertarClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                DocumentoIdentidad = dto.DocumentoIdentidad
            };

            await _clienteService.AddAsync(cliente);
            return Ok(new { message = "Cliente registrado correctamente" });
        }

        [HttpPut("ModifyClient")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Cliente cliente)
        {
            cliente.IdCliente = id;
            await _clienteService.UpdateAsync(cliente);
            return Ok(new { message = "Cliente modificado correctamente" });
        }

        [HttpDelete("DeleteClient")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _clienteService.DeleteAsync(id);
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
