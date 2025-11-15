using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HotelReservation.Application.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly HotelReservationDBContext _db;
        private const decimal ImpuestoPct = 0.12m; // 12% como ejemplo

        public FacturaService(HotelReservationDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FacturaDto>> ListarFacturasAsync()
        {
            var entities = await _db.Facturas
                .Include(f => f.Detalles)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync();

            return entities.Select(MapToDto);
        }

        public async Task<Guid> GenerarFacturaAsync(Guid reservaId, DateTime? checkIn, DateTime? checkOut, string huespedNombre, IEnumerable<(string descripcion, int cantidad, decimal precio)> detalles, string metodoPago)
        {
            var factura = new Factura
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaEmision = DateTime.UtcNow,
                MetodoPago = metodoPago ?? string.Empty,
            };

            // Crear detalles
            decimal subtotal = 0m;
            if (detalles != null)
            {
                foreach (var d in detalles)
                {
                    var det = new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = factura.Id,
                        Descripcion = d.descripcion,
                        Cantidad = d.cantidad,
                        PrecioUnitario = d.precio,
                        Subtotal = d.precio * d.cantidad
                    };
                    factura.Detalles.Add(det);
                    subtotal += det.Subtotal;
                }
            }

            // Ejemplo: si quieres agregar tarifa por noches desde reserva, puedes consultar _db.Reservas
            // Para ahora, asumimos subtotal contiene todo
            var impuestos = Math.Round(subtotal * ImpuestoPct, 2);
            factura.MontoTotal = subtotal + impuestos;

            // Campos opcionales de check-in/out/huesped
            if (checkIn.HasValue)
            {
                // si tu entidad Factura no tiene CheckIn/CheckOut, omite o añade como necesites
                // en tus entities no aparecían, por eso en DTO son opcionales
            }

            _db.Facturas.Add(factura);
            await _db.SaveChangesAsync();

            return factura.Id;
        }

        public async Task<FacturaDto?> ObtenerFacturaAsync(Guid facturaId)
        {
            var f = await _db.Facturas
                .Include(x => x.Detalles)
                .FirstOrDefaultAsync(x => x.Id == facturaId);

            if (f == null) return null;
            return MapToDto(f);
        }

        public async Task<byte[]> GenerarPdfFacturaAsync(Guid facturaId)
        {
            var f = await _db.Facturas.Include(x => x.Detalles).FirstOrDefaultAsync(x => x.Id == facturaId);
            if (f == null) throw new KeyNotFoundException("Factura no encontrada");

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Hotel Reservation").FontSize(18).Bold();
                            col.Item().Text("Factura").FontSize(14).SemiBold();
                        });
                        row.ConstantItem(150).AlignRight().Text($"Fecha: {f.FechaEmision:u}");
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(5);

                        col.Item().Text($"Factura: {f.Id}").SemiBold();
                        col.Item().Text($"Reserva: {f.ReservaId}");
                        col.Item().Text($"Método de pago: {f.MetodoPago}");
                        if (f.Detalles != null && f.Detalles.Any())
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(60);
                                    columns.ConstantColumn(80);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Descripción");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Cant.");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Subtotal");
                                });

                                foreach (var d in f.Detalles)
                                {
                                    table.Cell().Element(CellCell).Text(d.Descripcion);
                                    table.Cell().Element(CellCell).AlignRight().Text(d.Cantidad.ToString());
                                    table.Cell().Element(CellCell).AlignRight().Text($"{d.Subtotal:C}");
                                }

                                table.Footer(footer =>
                                {
                                    footer.Cell().Element(CellCell).Text("TOTAL");
                                    footer.Cell().Element(CellCell).Text("");
                                    footer.Cell().Element(CellCell).AlignRight().Text($"{f.MontoTotal:C}");
                                });

                                static IContainer CellStyle(IContainer c) => c.Padding(5).DefaultTextStyle(x => x.SemiBold());
                                static IContainer CellCell(IContainer c) => c.Padding(5);
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text("Gracias por su preferencia");
                });
            });

            using var ms = new System.IO.MemoryStream();
            doc.GeneratePdf(ms);
            return ms.ToArray();
        }

        public async Task<IEnumerable<object>> ReporteOcupacionAsync(DateTime desde, DateTime hasta)
        {
            // Ejemplo básico: número de facturas por día por CheckIn (si tienes datos de CheckIn)
            var list = await _db.Facturas
                .Where(f => f.FechaEmision.Date >= desde.Date && f.FechaEmision.Date <= hasta.Date)
                .ToListAsync();

            var grouped = list
                .GroupBy(x => x.FechaEmision.Date)
                .Select(g => new { Fecha = g.Key, Reservas = g.Count() })
                .OrderBy(x => x.Fecha)
                .ToList<object>();

            return grouped;
        }

        public async Task<IEnumerable<object>> ReporteIngresosAsync(DateTime desde, DateTime hasta)
        {
            var list = await _db.Facturas
                .Where(f => f.FechaEmision.Date >= desde.Date && f.FechaEmision.Date <= hasta.Date)
                .ToListAsync();

            var grouped = list
                .GroupBy(x => x.FechaEmision.Date)
                .Select(g => new { Fecha = g.Key, Ingresos = g.Sum(x => x.MontoTotal) })
                .OrderBy(x => x.Fecha)
                .ToList<object>();

            return grouped;
        }

        private static FacturaDto MapToDto(Factura f)
        {
            return new FacturaDto
            {
                Id = f.Id,
                ReservaId = f.ReservaId,
                FechaEmision = f.FechaEmision,
                MontoTotal = f.MontoTotal,
                MetodoPago = f.MetodoPago,
                Detalles = f.Detalles.Select(d => new DetalleFacturaDto
                {
                    Id = d.Id,
                    FacturaId = d.FacturaId,
                    Descripcion = d.Descripcion,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }
}