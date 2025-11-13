using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Services; // Usando el servicio existente
using HotelReservation.Application.DTOs;     // Usando el DTO existente
using HotelReservation.Domain.Entities;     // Necesario para HabitacionStatus
using System.ComponentModel.DataAnnotations;

// --- DTOs Mínimos ---
// Estos DTOs no existen en tus archivos, pero son necesarios para 
// recibir datos del body en los endpoints de POST y PUT.
// Deberás crearlos en la carpeta Application/Dtos.

public record InsertarHabitacionDto(
    [Required] int Number, 
    [Required] Guid CategoryId, 
    [Required][Range(0.01, 999999)] decimal Price
);
public record ActualizarPrecioDto([Required][Range(0.01, 999999)] decimal Price);
public record ActualizarEstadoDto([Required] HabitacionStatus Status);

// ---------------------

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitacionesController : ControllerBase
    {
        // Inyecta el servicio CONCRETO existente
        private readonly HabitacionService _service; 
        private readonly ILogger<HabitacionesController> _logger;

        // Debes asegurarte de registrar 'HabitacionService' en tu DI (IoC)
        public HabitacionesController(HabitacionService service, ILogger<HabitacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API - GetAll Habitaciones (servicio existente)");
            var habitaciones = await _service.GetAllAsync(); //
            return Ok(habitaciones);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("API - GetHabitacionById {Id} (servicio existente)", id);
            var habitacion = await _service.GetHabitacionAsync(id); //
            
            if (habitacion == null) 
                return NotFound("Habitación no encontrada.");
                
            return Ok(habitacion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InsertarHabitacionDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                // Llama al método existente que toma parámetros separados
                var habitacionDto = await _service.CreateHabitacionAsync(dto.Number, dto.CategoryId, dto.Price); 
                return CreatedAtAction(nameof(GetById), new { id = habitacionDto.Id }, habitacionDto);
            }
            catch (InvalidOperationException ex) // Ej: "Ya existe una habitación con ese número"
            {
                _logger.LogWarning(ex, "Error al crear habitación {Number}", dto.Number);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear habitación {Number}", dto.Number);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id:guid}/price")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] ActualizarPrecioDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
                
            try
            {
                await _service.UpdatePriceAsync(id, dto.Price); //
                return NoContent(); // 204 No Content es estándar para un PUT exitoso sin respuesta
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Habitación no encontrada.");
            }
        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ActualizarEstadoDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

             try
            {
                await _service.ChangeStatusAsync(id, dto.Status); //
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Habitación no encontrada.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id); //
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                // El servicio lanza esto si no la encuentra
                return NotFound("Habitación no encontrada.");
            }
        }
    }
}