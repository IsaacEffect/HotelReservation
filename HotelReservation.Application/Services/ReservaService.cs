using System;
using System.Collections.Generic;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
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

        //  Crear una nueva reserva
        public Guid CrearReserva(CrearReservaDTO dto)
        {
            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("Las fechas de inicio y fin son inválidas.");

            if (!_repo.HabitacionDisponible(dto.HabitacionId, dto.FechaInicio, dto.FechaFin))
                throw new Exception("La habitación no está disponible en ese rango de fechas.");

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

        //  Obtener todas las reservas simples
        public IEnumerable<Reserva> ObtenerReservas()
        {
            return _repo.ObtenerReservas();
        }

        // NUEVO MÉTODO - Obtener reservas con información detallada
        public IEnumerable<object> ObtenerReservasConDetalles()
        {
            return _repo.ObtenerReservasConDetalles();
        }
    }
}
