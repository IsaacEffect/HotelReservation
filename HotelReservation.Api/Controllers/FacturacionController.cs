using Microsoft.AspNetCore.Mvc;
using HotelReservation.Application.Contracts;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Listar()
        {
            var items = await _facturaService.ListarAsync();
            return Ok(items);
        }

        [HttpGet("detalle/{id:guid}")]
        public async Task<IActionResult> Detalle(Guid id)
        {
            var item = await _facturaService.ObtenerPorIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearFacturaRequest req)
        {
            if (req == null || req.ReservaId == Guid.Empty) return BadRequest("ReservaId inválido.");

            var id = await _facturaService.CrearFacturaDesdeReservaAsync(req.ReservaId, req.MetodoPago);
            return CreatedAtAction(nameof(Detalle), new { id }, new { id });
        }

        [HttpGet("pdf/{id:guid}")]
        public async Task<IActionResult> Pdf(Guid id)
        {
            var bytes = await _facturaService.GenerarPdfAsync(id);
            return File(bytes, "application/pdf", $"factura_{id}.pdf");
        }

        public class CrearFacturaRequest
        {
            public Guid ReservaId { get; set; }
            public string MetodoPago { get; set; } = "Efectivo";
        }
    }
}