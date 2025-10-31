using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace HotelReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly HotelReservationContext _context;

        public TestController(HotelReservationContext context)
        {
            _context = context;
        }

        [HttpPost("insertar-detallefactura")]
        public async Task<IActionResult> InsertarDetalleFacturaManual()
        {
            var facturaIdExistente = await _context.Facturas
                .Select(f => f.Id)
                .FirstOrDefaultAsync();

            if (facturaIdExistente == Guid.Empty)
                return BadRequest("No hay facturas en la base de datos.");

            var detalle = new DetalleFactura
            {
                Id = Guid.NewGuid(),
                FacturaId = facturaIdExistente,
                Descripcion = "Servicio de prueba",
                Cantidad = 1,
                PrecioUnitario = 100,
                Subtotal = 100
            };

            try
            {
                _context.DetalleFacturas.Add(detalle);
                await _context.SaveChangesAsync();
                return Ok("DetalleFactura insertado correctamente.");
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error al insertar: {inner}");
            }
        }
    }
}
