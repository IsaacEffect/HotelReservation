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
                ReservaId = GetValue<Guid>(r, "Id"),
                FechaReserva = GetValue<DateTime>(r, "FechaReserva"),
                FechaInicio = GetValue<DateTime>(r, "FechaInicio"),
                FechaFin = GetValue<DateTime>(r, "FechaFin"),
                EstadoReserva = GetValue<string>(r, "EstadoReserva"),
                Cliente = GetValue<string>(r, "NombreCliente"),
                CorreoCliente = GetValue<string>(r, "CorreoCliente"),
                NumeroHabitacion = GetValue<string>(r, "NumeroHabitacion"),
                Categoria = GetValue<string>(r, "Categoria"),
                PrecioPorNoche = GetValue<decimal>(r, "PrecioPorNoche"),
                UsuarioRegistro = GetValue<string>(r, "NombreUsuario"),
                Total = GetOptional<decimal>(r, "Total")
            });
        }

        // READ - Detalle por ID
        public async Task<ReservaDetalleDTO?> GetReservaDetalleByIdAsync(Guid id)
        {
            var reservas = await _repo.ObtenerReservasConDetallesAsync();
            var r = reservas.FirstOrDefault(x => GetValue<Guid>(x, "Id") == id);

            if (r == null) return null;

            return new ReservaDetalleDTO
            {
                ReservaId = GetValue<Guid>(r, "Id"),
                FechaReserva = GetValue<DateTime>(r, "FechaReserva"),
                FechaInicio = GetValue<DateTime>(r, "FechaInicio"),
                FechaFin = GetValue<DateTime>(r, "FechaFin"),
                EstadoReserva = GetValue<string>(r, "EstadoReserva"),
                Cliente = GetValue<string>(r, "NombreCliente"),
                CorreoCliente = GetValue<string>(r, "CorreoCliente"),
                NumeroHabitacion = GetValue<string>(r, "NumeroHabitacion"),
                Categoria = GetValue<string>(r, "Categoria"),
                PrecioPorNoche = GetValue<decimal>(r, "PrecioPorNoche"),
                UsuarioRegistro = GetValue<string>(r, "NombreUsuario"),
                Total = GetOptional<decimal>(r, "Total")
            };
        }

        // UPDATE - Actualizar fechas
        public async Task ActualizarReservaAsync(Guid id, ActualizarReservaDTO dto)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(id);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin");

            var disponible = await _repo.HabitacionDisponibleAsync(
                reserva.HabitacionId,
                dto.FechaInicio,
                dto.FechaFin,
                id);

            if (!disponible)
                throw new InvalidOperationException("La habitación no está disponible para las nuevas fechas");

            reserva.FechaInicio = dto.FechaInicio;
            reserva.FechaFin = dto.FechaFin;

            // Obtener precio por noche
            var precio = await _repo.ObtenerPrecioHabitacionAsync(reserva.HabitacionId);

            // Calcular días
            int dias = (int)(dto.FechaFin - dto.FechaInicio).TotalDays;
            if (dias <= 0)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");

            // Guardar total calculado
            reserva.Total = dias * precio;


            await _repo.ModificarReservaAsync(reserva);
        }

        // UPDATE - Cambiar estado
        public async Task CambiarEstadoReservaAsync(Guid id, ActualizarEstadoReservaDTO dto)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(id);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            if (string.IsNullOrWhiteSpace(dto.NuevoEstado))
                throw new ArgumentException("El nuevo estado no puede ser nulo o vacío");

            var estadosValidos = new[] { "Activa", "Pendiente", "Confirmada", "Cancelada", "Completada" };

            if (!estadosValidos.Contains(dto.NuevoEstado))
                throw new ArgumentException("Estado de reserva inválido");

            reserva.EstadoReserva = dto.NuevoEstado;
            await _repo.ModificarReservaAsync(reserva);
        }

        // DELETE - Cancelar (estado lógico)
        public async Task CancelarReservaAsync(Guid reservaId)
        {
            var reserva = await _repo.ObtenerReservaPorIdAsync(reservaId);
            if (reserva == null)
                throw new KeyNotFoundException("Reserva no encontrada");

            await _repo.CancelarReservaAsync(reservaId);
        }

        // Verificar disponibilidad
        public async Task<bool> VerificarDisponibilidadAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _repo.HabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin);
        }

        // Helper seguro para propiedades obligatorias
        private T GetValue<T>(object obj, string property)
        {
            var value = obj.GetType().GetProperty(property)?.GetValue(obj);

            if (value == null)
                throw new NullReferenceException($"La propiedad '{property}' vino nula.");

            return (T)value;
        }

        // Helper seguro para valores opcionales
        private T? GetOptional<T>(object obj, string property) where T : struct
        {
            var value = obj.GetType().GetProperty(property)?.GetValue(obj);
            return value == null ? (T?)null : (T)value;
        }
    }
}
