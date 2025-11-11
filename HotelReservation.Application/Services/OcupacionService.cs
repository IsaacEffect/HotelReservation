using HotelReservation.Application.Contracts;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class OcupacionService : IOcupacionService
    {
        private readonly IHabitacionRepository _habitacionRepo;

        public OcupacionService(IHabitacionRepository habitacionRepo)
        {
            _habitacionRepo = habitacionRepo;
        }

        public async Task<OcupacionDiariaDto> ObtenerOcupacionDiariaAsync()
        {
            var habitaciones = await _habitacionRepo.GetAllAsync();

            var total = habitaciones.Count();
            var ocupadas = habitaciones.Count(h => h.Status == HabitacionStatus.Occupied);
            var disponibles = habitaciones.Count(h => h.Status == HabitacionStatus.Available);
            var mantenimiento = habitaciones.Count(h => h.Status == HabitacionStatus.Maintenance);

            return new OcupacionDiariaDto
            {
                Fecha = DateTime.UtcNow,
                TotalHabitaciones = total,
                Ocupadas = ocupadas,
                Disponibles = disponibles,
                Mantenimiento = mantenimiento,
                PorcentajeOcupacion = total > 0 ? Math.Round((ocupadas * 100.0) / total, 2) : 0
            };
        }
    }
}