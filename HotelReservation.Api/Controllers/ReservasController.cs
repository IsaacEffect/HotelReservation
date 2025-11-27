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

        // CREATE - POST: Crear nueva reserva
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDTO dto)
        {
            try
            {
                var id = await _service.CrearReservaAsync(dto);
                return Created($"/api/reservas/{id}", new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // READ - GET: Listar todas las reservas básicas
        [HttpGet("GetAllReservations")]
        public async Task<IActionResult> ListarReservas()
        {
            try
            {
                var reservas = await _service.GetAllAsync();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // READ - GET: Obtener reserva por ID
        [HttpGet("GetReservationById/{id:guid}")]
        public async Task<IActionResult> ObtenerReservaPorId(Guid id)
        {
            try
            {
                var reserva = await _service.GetByIdAsync(id);
                if (reserva == null)
                    return NotFound(new { error = "Reserva no encontrada" });

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // READ - GET: Listar reservas con detalles completos (JOIN)
        [HttpGet("GetAllReservationsWithDetails")]
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

        // READ - GET: Obtener detalle completo de reserva por ID
        [HttpGet("GetReservationDetailsById/{id:guid}")]
        public async Task<IActionResult> ObtenerReservaDetallePorId(Guid id)
        {
            try
            {
                var reserva = await _service.GetReservaDetalleByIdAsync(id);
                if (reserva == null)
                    return NotFound(new { error = "Reserva no encontrada" });

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // UPDATE - PUT: Actualizar fechas de reserva
        [HttpPut("UpdateReservation/{id:guid}")]
        public async Task<IActionResult> ActualizarReserva(Guid id, [FromBody] ActualizarReservaDTO dto)
        {
            try
            {
                await _service.ActualizarReservaAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // UPDATE - PATCH: Cambiar estado de reserva
        [HttpPatch("UpdateReservationStatus/{id:guid}")]
        public async Task<IActionResult> CambiarEstadoReserva(Guid id, [FromBody] ActualizarEstadoReservaDTO dto)
        {
            try
            {
                await _service.CambiarEstadoReservaAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE - DELETE: Cancelar reserva
        [HttpDelete("CancelReservation/{id:guid}")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            try
            {
                await _service.CancelarReservaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Utilidad - GET: Verificar disponibilidad de habitación
        [HttpGet("CheckHabitacionDisponibilidad")]
        public async Task<IActionResult> VerificarDisponibilidad(
            [FromQuery] Guid habitacionId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                var disponible = await _service.VerificarDisponibilidadAsync(habitacionId, fechaInicio, fechaFin);
                return Ok(new { disponible });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}