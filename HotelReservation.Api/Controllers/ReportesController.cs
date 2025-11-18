using HotelReservation.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;
        private readonly IOcupacionService _ocupacionService;

        public ReportesController(IReporteService reporteService, IOcupacionService ocupacionService)
        {
            _reporteService = reporteService;
            _ocupacionService = ocupacionService;
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

        [HttpGet("ocupacion-diaria")]
        public async Task<IActionResult> ObtenerOcupacionDiaria()
        {
            var reporte = await _ocupacionService.ObtenerOcupacionDiariaAsync();
            return Ok(reporte);
        }
    }
}