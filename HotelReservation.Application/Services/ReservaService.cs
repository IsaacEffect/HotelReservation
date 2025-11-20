using HotelReservation.Application.Contracts;
using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repo;

        public ReservaService(IReservaRepository repo)
        {
            _repo = repo;
        }

        // CREATE - Crear nueva reserva
        public async Task<Guid> CrearReservaAsync(CrearReservaDTO dto)
        {
            if (dto.FechaInicio >= dto.FechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin");

            if (dto.FechaInicio < DateTime.Today)
                throw new ArgumentException("No se pueden crear reservas con fechas pasadas");

            var disponible = await _repo.HabitacionDisponibleAsync(dto.HabitacionId, dto.FechaInicio, dto.FechaFin);
            if (!disponible)
                throw new InvalidOperationException("La habitación no está disponible para las fechas seleccionadas");

            var reserva = new Reserva
            {
                ClienteId = dto.ClienteId,
                HabitacionId = dto.HabitacionId,
                UsuarioId = dto.UsuarioId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                EstadoReserva = "Pendiente"
            };

            return await _repo.CrearReservaAsync(reserva);
        }

        // READ - Obtener todas las reservas
        public async Task<IEnumerable<ReservaDTO>> GetAllAsync()
        {
            var reservas = await _repo.ObtenerReservasAsync();
            return reservas.Select(r => new ReservaDTO
            {
                Id = r.Id,
                FechaReserva = r.FechaReserva,
                FechaInicio = r.FechaInicio,
                FechaFin = r.FechaFin,
                EstadoReserva = r.EstadoReserva,
                ClienteId = r.ClienteId,
                HabitacionId = r.HabitacionId,
                UsuarioId = r.UsuarioId,
                Total = r.Total
            });
        }

        // READ - Obtener reserva por ID
        public async Task<ReservaDTO?> GetByIdAsync(Guid id)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(id);
            if (reserva == null) return null;

            return new ReservaDTO
            {
                Id = reserva.Id,
                FechaReserva = reserva.FechaReserva,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                EstadoReserva = reserva.EstadoReserva,
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                UsuarioId = reserva.UsuarioId,
                Total = reserva.Total
            };
        }

        // READ - Obtener reservas con detalles (JOIN)
        public async Task<IEnumerable<ReservaDetalleDTO>> ObtenerReservasConDetallesAsync()
        {
            var reservas = await _repo.ObtenerReservasConDetallesAsync();
            return reservas.Select(r => new ReservaDetalleDTO
            {
                ReservaId = (Guid)GetPropertyValue(r, "Id"),
                FechaReserva = (DateTime)GetPropertyValue(r, "FechaReserva"),
                FechaInicio = (DateTime)GetPropertyValue(r, "FechaInicio"),
                FechaFin = (DateTime)GetPropertyValue(r, "FechaFin"),
                EstadoReserva = GetPropertyValue(r, "EstadoReserva")?.ToString(),
                Cliente = GetPropertyValue(r, "NombreCliente")?.ToString(),
                CorreoCliente = GetPropertyValue(r, "CorreoCliente")?.ToString(),
                NumeroHabitacion = Convert.ToInt32(GetPropertyValue(r, "NumeroHabitacion")),
                Categoria = GetPropertyValue(r, "Categoria")?.ToString(),
                PrecioPorNoche = Convert.ToDecimal(GetPropertyValue(r, "PrecioPorNoche")),
                UsuarioRegistro = GetPropertyValue(r, "NombreUsuario")?.ToString(),
                Total = GetPropertyValue(r, "Total") != null ? Convert.ToDecimal(GetPropertyValue(r, "Total")) : null
            });
        }

        // READ - Obtener detalle de reserva por ID
        public async Task<ReservaDetalleDTO?> GetReservaDetalleByIdAsync(Guid id)
        {
            var reservas = await _repo.ObtenerReservasConDetallesAsync();
            var reserva = reservas.FirstOrDefault(r => (Guid)GetPropertyValue(r, "Id") == id);

            if (reserva == null) return null;

            return new ReservaDetalleDTO
            {
                ReservaId = (Guid)GetPropertyValue(reserva, "Id"),
                FechaReserva = (DateTime)GetPropertyValue(reserva, "FechaReserva"),
                FechaInicio = (DateTime)GetPropertyValue(reserva, "FechaInicio"),
                FechaFin = (DateTime)GetPropertyValue(reserva, "FechaFin"),
                EstadoReserva = GetPropertyValue(reserva, "EstadoReserva")?.ToString(),
                Cliente = GetPropertyValue(reserva, "NombreCliente")?.ToString(),
                CorreoCliente = GetPropertyValue(reserva, "CorreoCliente")?.ToString(),
                NumeroHabitacion = Convert.ToInt32(GetPropertyValue(reserva, "NumeroHabitacion")),
                Categoria = GetPropertyValue(reserva, "Categoria")?.ToString(),
                PrecioPorNoche = Convert.ToDecimal(GetPropertyValue(reserva, "PrecioPorNoche")),
                UsuarioRegistro = GetPropertyValue(reserva, "NombreUsuario")?.ToString(),
                Total = GetPropertyValue(reserva, "Total") != null ? Convert.ToDecimal(GetPropertyValue(reserva, "Total")) : null
            };
        }

        // UPDATE - Actualizar fechas de reserva
        public async Task ActualizarReservaAsync(ActualizarReservaDTO dto)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(dto.ReservaId);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin");

            var disponible = await _repo.HabitacionDisponibleAsync(
                reserva.HabitacionId,
                dto.FechaInicio,
                dto.FechaFin,
                dto.ReservaId);

            if (!disponible)
                throw new InvalidOperationException("La habitación no está disponible para las nuevas fechas");

            reserva.FechaInicio = dto.FechaInicio;
            reserva.FechaFin = dto.FechaFin;

            await _repo.ModificarReservaAsync(reserva);
        }

        // UPDATE - Cambiar estado de reserva
        public async Task CambiarEstadoReservaAsync(ActualizarEstadoReservaDTO dto)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(dto.ReservaId);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            var estadosValidos = new[] { "Activa", "Pendiente", "Confirmada", "Cancelada", "Completada" };
            if (!estadosValidos.Contains(dto.NuevoEstado))
                throw new ArgumentException("Estado de reserva inválido");

            reserva.EstadoReserva = dto.NuevoEstado;
            await _repo.ModificarReservaAsync(reserva);
        }

        // DELETE - Cancelar reserva (cambio de estado lógico)
        public async Task CancelarReservaAsync(Guid reservaId)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(reservaId);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            await _repo.CancelarReservaAsync(reservaId);
        }

        // Verificar disponibilidad de habitación
        public async Task<bool> VerificarDisponibilidadAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _repo.HabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin);
        }

        // Helper para obtener valores de propiedades dinámicas
        private object? GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }
    }
}