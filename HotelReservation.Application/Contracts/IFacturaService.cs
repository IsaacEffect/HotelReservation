using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;

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
