using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IFacturaService
    {
        Task<IEnumerable<FacturaDto>> ListarFacturasAsync();
        Task<Guid> GenerarFacturaAsync(Guid reservaId, DateTime? checkIn, DateTime? checkOut, string huespedNombre, IEnumerable<(string descripcion, int cantidad, decimal precio)> detalles, string metodoPago);
        Task<FacturaDto?> ObtenerFacturaAsync(Guid facturaId);
        Task<byte[]> GenerarPdfFacturaAsync(Guid facturaId);
        Task<IEnumerable<object>> ReporteOcupacionAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<object>> ReporteIngresosAsync(DateTime desde, DateTime hasta);
    }
}
