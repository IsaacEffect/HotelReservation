using HotelReservation.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IOcupacionService _ocupacionService;

        public ReportesController(IOcupacionService ocupacionService)
        {
            _ocupacionService = ocupacionService;
        }

        [HttpGet("ocupacion-diaria")]
        public async Task<IActionResult> ObtenerOcupacionDiaria()
        {
            var reporte = await _ocupacionService.ObtenerOcupacionDiariaAsync();
            return Ok(reporte);
        }
    }
}