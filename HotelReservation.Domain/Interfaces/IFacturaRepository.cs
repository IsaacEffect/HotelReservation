using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces
{
    public interface IFacturaRepository
    {
        Task<Factura> GetByIdAsync(Guid id);
        Task<IEnumerable<Factura>> GetAllAsync();
        Task AddAsync(Factura factura);
        Task AddDetalleAsync(DetalleFactura detalle);
        Task<bool> ExistsByReservaIdAsync(Guid reservaId);
    }
}
