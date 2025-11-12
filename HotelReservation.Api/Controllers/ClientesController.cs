using AutoMapper;
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
        private readonly IMapper _mapper;

        public ClientesController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            var clientesDto = _mapper.Map<IEnumerable<ObtenerClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        [HttpGet("GetClientById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(_mapper.Map<ObtenerClienteDto>(cliente));
        }

        [HttpPost("InsertClient")]
        public async Task<IActionResult> Create([FromBody] InsertarClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = _mapper.Map<Cliente>(dto);
            await _clienteService.AddAsync(_mapper.Map<InsertarClienteDto>(cliente));
            return Ok(new { message = "Cliente registrado correctamente" });
        }

        [HttpPut("ModifyClient")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clienteService.UpdateAsync(id, dto);
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
