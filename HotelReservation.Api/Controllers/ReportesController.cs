using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Interfaces.Services;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("ingresos")]
        public async Task<IActionResult> ObtenerIngresos([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var total = await _reporteService.ObtenerIngresosPorRangoAsync(desde, hasta);
            return Ok(new { ingresos = total });
        }

        [HttpGet("ocupacion")]
        public async Task<IActionResult> ObtenerOcupacion([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var cantidad = await _reporteService.ObtenerOcupacionPorRangoAsync(desde, hasta);
            return Ok(new { ocupacion = cantidad });
        }
    }
}