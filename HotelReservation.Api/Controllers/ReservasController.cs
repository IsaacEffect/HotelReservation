using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _service;

        public ReservasController(IReservaService service)
        {
            _service = service;
        }

        // POST: Crear nueva reserva
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDTO dto)
        {
            try
            {
                var id = await _service.CrearReservaAsync(dto);
                return Created($"/api/reservas/{id}", new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: Listar reservas básicas
        [HttpGet]
        public async Task<IActionResult> ListarReservas()
        {
            var reservas = await _service.ObtenerReservasAsync();
            return Ok(reservas);
        }

        // NUEVO ENDPOINT: Listar reservas detalladas
        [HttpGet("detalles")]
        public async Task<IActionResult> ListarReservasConDetalles()
        {
            try
            {
                var reservas = await _service.ObtenerReservasConDetallesAsync();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}