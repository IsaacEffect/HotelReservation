using HotelReservation.Api.Extensions;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitacionesController : ControllerBase
    {
        private readonly IHabitacionService _service;
        private readonly ILogger<HabitacionesController> _logger;

        public HabitacionesController(IHabitacionService service, ILogger<HabitacionesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("numero/{numero}")]
        public async Task<IActionResult> GetByNumber(string numero)
        {
            var result = await _service.GetByNumberAsync(numero);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InsertarHabitacionDto dto)
        {
            var result = await _service.AddAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarHabitacionDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpPatch("{id:guid}/estado")]
        public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] string estado)
        {
            var result = await _service.UpdateStatusAsync(id, estado);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}