using System;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Application.Contracts;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HotelReservation.Domain.Entities;
using HotelReservation.Infrastructure.Services;
using HotelReservation.Application.Dtos;

namespace HotelReservation.Application.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IUnitOfWork _uow;
        private readonly HotelReservationDBContext _context; // para la vista vw_ReservasDetalle
        private readonly IPdfService _pdfService;

        public FacturaService(IUnitOfWork uow, HotelReservationDBContext context, IPdfService pdfService)
        {
            _uow = uow;
            _context = context;
            _pdfService = pdfService;
        }

        public async Task<IEnumerable<FacturaDto>> ListarAsync()
        {
            var entidades = await _uow.Facturas.GetAllAsync();
            return entidades.Select(e => MapToDto(e)).ToList();
        }

        public async Task<FacturaDto> ObtenerPorIdAsync(Guid id)
        {
            var entidad = await _uow.Facturas.GetByIdAsync(id);
            if (entidad == null) return null;
            return MapToDto(entidad);
        }

        public async Task<Guid> CrearFacturaDesdeReservaAsync(Guid reservaId, string metodoPago)
        {
            // Validar que no exista factura para la reserva
            var existe = await _uow.Facturas.ExistsByReservaIdAsync(reservaId);
            if (existe) throw new InvalidOperationException("Ya existe factura para esta reserva.");

            // Obtener datos desde la vista vw_ReservasDetalle
            var reserva = await _context.ReservasDetalle
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId);

            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada.");

            int noches = Math.Max(1, (reserva.FechaFin - reserva.FechaInicio).Days);
            decimal totalHospedaje = noches * reserva.PrecioPorNoche;

            // Si deseas sumar servicios adicionales, consulta DetalleReserva o lógica adicional aquí.
            decimal montoTotal = totalHospedaje;

            // Crear entidad Factura
            var factura = new Factura
            {
                ReservaId = reservaId,
                MontoTotal = montoTotal,
                MetodoPago = metodoPago,
                FechaEmision = DateTime.UtcNow
            };

            await _uow.Facturas.AddAsync(factura);

            // Añadir detalle hospedaje
            var detalle = new DetalleFactura
            {
                FacturaId = factura.Id,
                Descripcion = $"Hospedaje ({noches} noche(s))",
                Cantidad = noches,
                PrecioUnitario = reserva.PrecioPorNoche
            };

            await _uow.Facturas.AddDetalleAsync(detalle);

            // Guardar
            await _uow.SaveChangesAsync();

            return factura.Id;
        }

        public async Task<byte[]> GenerarPdfAsync(Guid facturaId)
        {
            var dto = await ObtenerPorIdAsync(facturaId);
            if (dto == null) throw new InvalidOperationException("Factura no encontrada.");
            return _pdfService.GenerarFacturaPdf(dto);
        }

        // ---- helpers ----
        private FacturaDto MapToDto(Factura e)
        {
            var dto = new FacturaDto
            {
                Id = e.Id,
                ReservaId = e.ReservaId,
                FechaEmision = e.FechaEmision,
                MontoTotal = e.MontoTotal,
                MetodoPago = e.MetodoPago,
                Detalles = e.Detalles.Select(d => new DetalleFacturaDto
                {
                    Descripcion = d.Descripcion,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };
            return dto;
        }
    }
}
