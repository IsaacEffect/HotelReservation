using HotelReservation.Api.Extensions;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Administrador")] // Descomentar para proteger
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaHabitacionService _categoriaService;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaHabitacionService categoriaService, ILogger<CategoriasController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API - GetAll Categorias llamado.");
            var result = await _categoriaService.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("API - GetCategoriaById llamado para ID {Id}", id);
            var result = await _categoriaService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InsertarCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("API - Create Categoria llamado para {Name}", dto.Name);
            var result = await _categoriaService.AddAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("API - Update Categoria llamado para ID {Id}", id);
            var result = await _categoriaService.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("API - Delete Categoria llamado para ID {Id}", id);
            var result = await _categoriaService.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}