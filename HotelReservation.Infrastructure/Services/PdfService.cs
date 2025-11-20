using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HotelReservation.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerarFacturaPdf(FacturaDto factura)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("HOTEL RESERVATION").FontSize(18).Bold();
                            col.Item().Text("FACTURA").FontSize(14).SemiBold();
                        });

                        row.ConstantItem(180).Column(col =>
                        {
                            col.Item().Text($"Fecha: {factura.FechaEmision:yyyy-MM-dd}");
                            col.Item().Text($"Factura: {factura.Id}");
                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text($"Reserva: {factura.ReservaId}");
                        col.Item().Text($"Método de pago: {factura.MetodoPago}");
                        col.Item().Text(" ");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn();
                                c.ConstantColumn(60);
                                c.ConstantColumn(100);
                                c.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Descripción").SemiBold();
                                header.Cell().Text("Cant.").SemiBold();
                                header.Cell().Text("Precio Unit.").SemiBold();
                                header.Cell().Text("Subtotal").SemiBold();
                            });

                            foreach (var d in factura.Detalles)
                            {
                                table.Cell().Text(d.Descripcion);
                                table.Cell().Text(d.Cantidad.ToString());
                                table.Cell().Text(d.PrecioUnitario.ToString("N2"));
                                table.Cell().Text(d.Subtotal.ToString("N2"));
                            }

                            table.Footer(footer =>
                            {
                                footer.Cell().Text("");
                                footer.Cell().Text("");
                                footer.Cell().Text("TOTAL").SemiBold();
                                footer.Cell().Text(factura.MontoTotal.ToString("N2")).SemiBold();
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text("Gracias por su preferencia.");
                });
            });

            return document.GeneratePdf();
        }
    }
}