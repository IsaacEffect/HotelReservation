using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Contracts
{
    public interface IFacturaService
    {
        Task<IEnumerable<FacturaDto>> ListarAsync();
        Task<FacturaDto> ObtenerPorIdAsync(Guid id);
        Task<Guid> CrearFacturaDesdeReservaAsync(Guid reservaId, string metodoPago);
        Task<byte[]> GenerarPdfAsync(Guid facturaId);
    }
}