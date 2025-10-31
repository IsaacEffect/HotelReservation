using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Services;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaService _service;

        public ReservasController(ReservaService service)
        {
            _service = service;
        }

        //  POST: Crear nueva reserva
        [HttpPost]
        public IActionResult CrearReserva([FromBody] CrearReservaDTO dto)
        {
            try
            {
                var id = _service.CrearReserva(dto);
                return Created($"/api/reservas/{id}", new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //  GET: Listar reservas básicas
        [HttpGet]
        public IActionResult ListarReservas()
        {
            var reservas = _service.ObtenerReservas();
            return Ok(reservas);
        }

        //  NUEVO ENDPOINT: Listar reservas detalladas (Cliente, Habitación, Usuario)
        [HttpGet("detalles")]
        public IActionResult ListarReservasConDetalles()
        {
            try
            {
                var reservas = _service.ObtenerReservasConDetalles();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
