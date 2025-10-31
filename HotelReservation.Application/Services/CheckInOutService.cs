using System;
using System.Threading.Tasks;
using HotelReservation.Application.Interfaces.Services;
using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Application.Services
{
    public class CheckInOutService : ICheckInOutService
    {
        private readonly HotelReservationContext _context;

        public CheckInOutService(HotelReservationContext context)
        {
            _context = context;
        }

        public async Task RegistrarCheckInAsync(Guid reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            if (reserva == null)
                throw new Exception("Reserva no encontrada.");

            var checkIn = new CheckInOut
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaCheckIn = DateTime.Now,
                Observaciones = "Check-in registrado automáticamente"
            };

            reserva.Habitacion.Estado = "Ocupada";
            _context.CheckInOut.Add(checkIn);
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarCheckOutAsync(Guid reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Habitacion)
                .Include(r => r.Detalles) // DetalleReserva
                .FirstOrDefaultAsync(r => r.Id == reservaId);

            if (reserva == null)
                throw new Exception("Reserva no encontrada.");

            var checkOut = await _context.CheckInOut
                .FirstOrDefaultAsync(c => c.ReservaId == reservaId);

            if (checkOut == null)
                throw new Exception("No se encontró registro de Check-In.");

            checkOut.FechaCheckOut = DateTime.Now;
            reserva.Habitacion.Estado = "Disponible";
            reserva.EstadoReserva = "Completada";

            // Calcular total
            var noches = (reserva.FechaFin - reserva.FechaInicio).Days;
            var precioPorNoche = await _context.Habitaciones
                .Where(h => h.Id == reserva.HabitacionId)
                .Select(h => h.Categoria.PrecioPorNoche)
                .FirstOrDefaultAsync();

            var totalServicios = reserva.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario); // Subtotal calculado
            var totalEstancia = noches * precioPorNoche;
            var totalFinal = totalEstancia + totalServicios;

            reserva.Total = totalFinal;

            // Generar factura
            var factura = new Factura
            {
                Id = Guid.NewGuid(),
                ReservaId = reservaId,
                FechaEmision = DateTime.Now,
                MetodoPago = "Efectivo",
                MontoTotal = totalFinal
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync(); // Guarda la factura primero
            Console.WriteLine($"Factura guardada con ID: {factura.Id}");

            // Insertar detalles de factura
            Console.WriteLine($"Detalles encontrados: {reserva.Detalles.Count}");

            foreach (var detalle in reserva.Detalles)
            {
                var detalleFactura = new DetalleFactura
                {
                    Id = Guid.NewGuid(),
                    FacturaId = factura.Id,
                    Descripcion = detalle.Descripcion,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                    // Subtotal se calcula automáticamente en SQL Server
                };

                _context.DetalleFacturas.Add(detalleFactura);
            }

            try
            {
                await _context.SaveChangesAsync(); // Guarda los detalles después
                Console.WriteLine("Detalles de factura guardados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar DetalleFactura: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }
    }
}