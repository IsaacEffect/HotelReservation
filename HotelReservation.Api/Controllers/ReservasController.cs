using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Puedes descomentar esto si ya tienes JWT implementado
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _service;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(IReservaService service, ILogger<ReservasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// 1. CREAR una nueva reserva.
        /// (Implementa tu Lógica de Audio 1: Validar y Crear)
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var id = await _service.CrearReservaAsync(dto);
                // Retorna 201 Created con la ubicación del nuevo recurso
                return CreatedAtAction(nameof(ObtenerReservaPorId), new { id = id }, new { id });
            }
            catch (InvalidOperationException ex) // Captura la validación de disponibilidad
            {
                _logger.LogWarning(ex, "Intento de crear reserva en habitación no disponible.");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la reserva.");
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// 2. MODIFICAR una reserva existente (fechas).
        /// (Implementa tu Lógica de Audio 2: Modificar y poner en "Pendiente")
        /// </summary>
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarReserva([FromBody] ActualizarReservaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.ActualizarReservaAsync(dto);
                // El servicio ya la puso en "Pendiente"
                return Ok(new { success = true, message = "Reserva actualizada y movida a 'Pendiente' para revisión." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la reserva {ReservaId}", dto.ReservaId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// 3. CANCELAR una reserva.
        /// (Implementa tu Lógica de Audio 3: Cambiar estado a "Cancelada")
        /// </summary>
        [HttpPut("cancelar/{id:guid}")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            try
            {
                await _service.CancelarReservaAsync(id);
                return Ok(new { success = true, message = "Reserva cancelada exitosamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la reserva {ReservaId}", id);
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// 4. CAMBIAR ESTADO de una reserva (Ej: de Pendiente a Confirmada).
        /// (Implementa tu Lógica de Audio 4: Usar el DTO para cambiar estado)
        /// </summary>
        [HttpPut("cambiar-estado")]
        public async Task<IActionResult> CambiarEstadoReserva([FromBody] ActualizarEstadoReservaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.CambiarEstadoReservaAsync(dto);
                return Ok(new { success = true, message = $"Estado de la reserva cambiado a '{dto.NuevoEstado}'." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de la reserva {ReservaId}", dto.ReservaId);
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// 5. VALIDAR DISPONIBILIDAD (Endpoint para el Front-End).
        /// (Implementa tu Lógica de Audio 1: El paso de validación)
        /// </summary>
        [HttpGet("validar")]
        public async Task<IActionResult> ValidarDisponibilidad(
            [FromQuery] Guid habitacionId,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            if (habitacionId == Guid.Empty || fechaInicio == default || fechaFin == default)
                return BadRequest("Habitación, fecha de inicio y fecha de fin son requeridas.");

            var disponible = await _service.VerificarDisponibilidadAsync(habitacionId, fechaInicio, fechaFin);

            return Ok(new { disponible = disponible });
        }

        /// <summary>
        /// OBTENER todas las reservas (CRUD Básico).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodasLasReservas()
        {
            var reservas = await _service.GetAllAsync();
            return Ok(reservas);
        }

        /// <summary>
        /// OBTENER una reserva por ID (CRUD Básico).
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObtenerReservaPorId(Guid id)
        {
            var reserva = await _service.GetByIdAsync(id);
            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }
    }
}