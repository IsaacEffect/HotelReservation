using System;
using System.Collections.Generic; // Agregue esto para usar IEnumerable<Reserva>
using HotelReservation.Domain;
using HotelReservation.Persistence.Repositories; 

namespace HotelReservation.Application.Services
{
    public class ReservaService
    {
        private readonly ReservaRepository _repo;

        public ReservaService(ReservaRepository repo)
        {
            _repo = repo;
        }

        // El método que ya tiene para crear
        public Guid CrearReserva(CrearReservaDTO dto)
        {
            // ... (su lógica de CrearReserva) ...
            
            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("Fechas inválidas.");

            if (!_repo.HabitacionDisponible(dto.HabitacionId, dto.FechaInicio, dto.FechaFin))
                throw new Exception("La habitación no está disponible en ese rango.");

            var noches = (dto.FechaFin - dto.FechaInicio).Days;
            var total = dto.PrecioPorNoche * noches;

            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                EstadoReserva = "Pendiente",
                ClienteId = dto.ClienteId,
                HabitacionId = dto.HabitacionId,
                UsuarioId = dto.UsuarioId,
                Total = total
            };

            return _repo.CrearReserva(reserva);
        }

       
        public IEnumerable<Reserva> ObtenerReservas()
        {
        
            return _repo.ObtenerReservas(); 
        }
    }
}