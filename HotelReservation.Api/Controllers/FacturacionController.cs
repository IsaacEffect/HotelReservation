using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturacionController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturacionController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarFacturas()
        {
            var facturas = await _facturaService.ListarFacturasAsync();
            return Ok(facturas);
        }

        public class GenerarFacturaRequest
        {
            public Guid ReservaId { get; set; }
            public DateTime? CheckIn { get; set; }
            public DateTime? CheckOut { get; set; }
            public string HuespedNombre { get; set; } = string.Empty;
            public string MetodoPago { get; set; } = "Efectivo";
            public List<ServicioDto>? Servicios { get; set; }
        }

        public class ServicioDto
        {
            public string Descripcion { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
        }

        [HttpPost("generar")]
        public async Task<IActionResult> GenerarFactura([FromBody] GenerarFacturaRequest request)
        {
            if (request == null) return BadRequest("Solicitud vacía");
            var detalles = request.Servicios?.Select(s => (s.Descripcion, s.Cantidad, s.Precio)) ?? Enumerable.Empty<(string, int, decimal)>();

            var id = await _facturaService.GenerarFacturaAsync(request.ReservaId, request.CheckIn, request.CheckOut, request.HuespedNombre, detalles, request.MetodoPago);
            return Ok(new { FacturaId = id });
        }

        [HttpGet("{facturaId:guid}")]
        public async Task<IActionResult> ObtenerFactura(Guid facturaId)
        {
            var dto = await _facturaService.ObtenerFacturaAsync(facturaId);
            return dto != null ? Ok(dto) : NotFound();
        }

        [HttpGet("pdf/{facturaId:guid}")]
        public async Task<IActionResult> ObtenerPdf(Guid facturaId)
        {
            var pdf = await _facturaService.GenerarPdfFacturaAsync(facturaId);
            return File(pdf, "application/pdf", $"Factura_{facturaId}.pdf");
        }

        [HttpGet("report/ocupacion")]
        public async Task<IActionResult> ReporteOcupacion([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var r = await _facturaService.ReporteOcupacionAsync(desde, hasta);
            return Ok(r);
        }

        [HttpGet("report/ingresos")]
        public async Task<IActionResult> ReporteIngresos([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var r = await _facturaService.ReporteIngresosAsync(desde, hasta);
            return Ok(r);
        }
    }
}
