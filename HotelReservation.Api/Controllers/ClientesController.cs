using HotelReservation.Application.Contracts;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            return cliente is null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            await _clienteService.AddAsync(cliente);
            return Ok(new { message = "Cliente registrado correctamente" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Cliente cliente)
        {
            cliente.IdCliente = id;
            await _clienteService.UpdateAsync(cliente);
            return Ok(new { message = "Cliente modificado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _clienteService.DeleteAsync(id);
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
