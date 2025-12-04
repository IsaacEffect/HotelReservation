using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services
{
    public interface ICheckInOutService
    {
        Task<CheckInOutDto?> GetByReservaIdAsync(Guid reservaId);
        Task<CheckInOutDto> RegisterCheckInAsync(CreateCheckInRequest request);
        Task<CheckInOutDto> RegisterCheckOutAsync(CreateCheckOutRequest request);
        Task<IEnumerable<CheckInOutDto>> GetAllAsync();
        Task<CheckInOutDto?> GetByIdAsync(Guid id);
        Task<CheckInOutDto?> UpdateAsync(Guid id, UpdateCheckInOutRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class CheckInOutService : ICheckInOutService
    {
        private readonly ICheckInOutRepository _checkRepo;
        private readonly IHistorialReservaRepository _histRepo;
        private readonly IReservaRepository _reservaRepo;
        private readonly IHabitacionRepository _habitacionRepo; // para actualizar el estado
        private readonly IClienteRepository _clienteRepo;

        public CheckInOutService(
            ICheckInOutRepository checkRepo,
            IHistorialReservaRepository histRepo,
            IReservaRepository reservaRepo,
            IHabitacionRepository habitacionRepo,
            IClienteRepository clienteRepo)
        {
            _checkRepo = checkRepo;
            _histRepo = histRepo;
            _reservaRepo = reservaRepo;
            _habitacionRepo = habitacionRepo;
            _clienteRepo = clienteRepo;
        }

        public async Task<CheckInOutDto?> GetByReservaIdAsync(Guid reservaId)
        {
            var ent = await _checkRepo.GetByReservaIdAsync(reservaId);
            if (ent == null) return null;
            return new CheckInOutDto
            {
                Id = ent.Id,
                ReservaId = ent.ReservaId,
                FechaCheckIn = ent.FechaCheckIn,
                FechaCheckOut = ent.FechaCheckOut,
                Observaciones = ent.Observaciones
            };
        }

        public async Task<CheckInOutDto> RegisterCheckInAsync(CreateCheckInRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "La solicitud de Check-In no puede ser nula.");

            if (request.ReservaId == Guid.Empty)
                throw new ArgumentException("El Id de la reserva no es valido.", nameof(request.ReservaId));

            var check = new CheckInOut
            {
                ReservaId = request.ReservaId,
                FechaCheckIn = request.FechaCheckIn,
                Observaciones = request.Observaciones
            };

            var created = await _checkRepo.AddAsync(check);

            // Actualiza el estado de la habitacion a 'Ocupada'
            if (_reservaRepo != null && _habitacionRepo != null)
            {
                var reserva = await _reservaRepo.ObtenerReservaPorIdAsync(request.ReservaId);
                if (reserva != null)
                {
                    // Actualizar estado de habitacion
                    var habitacion = await _habitacionRepo.GetByIdAsync(reserva.HabitacionId);
                    if (habitacion != null)
                    {
                        if (habitacion.Estado == "Mantenimiento")
                            throw new InvalidOperationException("No se puede realizar el check-in: la habitacion se encuentra en mantenimiento.");

                        if (habitacion.Estado == "Ocupada")
                            throw new InvalidOperationException("La habitación ya se encuentra ocupada.");

                        try
                        {
                            habitacion.Estado = "Ocupada";
                            await _habitacionRepo.UpdateAsync(habitacion);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException($"Error al actualizar el estado de la habitacion ({habitacion.Numero}).", ex);
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException("No se encontro la habitacion asociada a la reserva.");
                    }
                }
                else
                {
                    throw new KeyNotFoundException("No se encontro la reserva asociada al Check-In.");
                }
            }
            else
            {
                throw new InvalidOperationException("Repositorios de Reserva o Habitacion no disponibles.");
            }

            return new CheckInOutDto
            {
                Id = created.Id,
                ReservaId = created.ReservaId,
                FechaCheckIn = created.FechaCheckIn,
                Observaciones = created.Observaciones
            };
        }

        public async Task<CheckInOutDto> RegisterCheckOutAsync(CreateCheckOutRequest request)
        {

            if (request == null)
                throw new ArgumentNullException(nameof(request), "La solicitud de CheckOut no puede ser nula.");

            if (request.ReservaId == Guid.Empty)
                throw new ArgumentException("El Id de la reserva no es valido.", nameof(request.ReservaId));

            // Buscar el registro existente de CheckInOut
            var existing = await _checkRepo.GetByReservaIdAsync(request.ReservaId);

            // Verificar si ya tiene un Check-Out registrado
            if (existing != null && existing.FechaCheckOut != null)
                throw new InvalidOperationException("Ya existe un Check-Out registrado para esta reserva.");

            if (existing == null)
            {
                // Si no existe, crea uno nuevo solo con CheckOut
                existing = new CheckInOut
                {
                    ReservaId = request.ReservaId,
                    FechaCheckOut = request.FechaCheckOut,
                    Observaciones = request.Observaciones
                };
                existing = await _checkRepo.AddAsync(existing);
            }
            else
            {
                // Actualiza el existente
                existing.FechaCheckOut = request.FechaCheckOut;
                existing.Observaciones = request.Observaciones;
                existing = await _checkRepo.UpdateAsync(existing);
            }

            // Obtener la reserva asociada
            var reserva = await _reservaRepo.ObtenerReservaPorIdAsync(request.ReservaId);
            if (reserva == null)
                throw new KeyNotFoundException("No se encontro la reserva asociada al Check-Out.");

            // Obtener la habitacion vinculada
            var habitacion = await _habitacionRepo.GetByIdAsync(reserva.HabitacionId);
            if (habitacion == null)
                throw new KeyNotFoundException("No se encontro la habitacion asociada a la reserva.");

            // Crear registro en HistorialReservas
            try
            {
                var historialExistente = await _histRepo.GetByClienteYFechasAsync(
                    reserva.ClienteId,
                    reserva.FechaInicio,
                    reserva.FechaFin
                );

                if (historialExistente == null)
                {
                    var historial = new HotelReservation.Domain.Entities.HistorialReserva
                    {
                        HabitacionId = reserva.HabitacionId,
                        ClienteId = reserva.ClienteId,
                        FechaEntrada = reserva.FechaInicio,
                        FechaSalida = reserva.FechaFin,
                        Motivo = "Estancia completada"
                    };
                    await _histRepo.AddAsync(historial);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el historial de la reserva.", ex);
            }

            // Actualizar el estado de la habitacion
            try
            {
                if (habitacion.Estado != "Mantenimiento")
                    habitacion.Estado = "Disponible";

                await _habitacionRepo.UpdateAsync(habitacion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el estado de la habitacion ({habitacion.Numero}).", ex);
            }

            // Actualizar el estado de la reserva
            try
            {
                reserva.EstadoReserva = "Completada";
                await _reservaRepo.ModificarReservaAsync(reserva);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado de la reserva a 'Completada'.", ex);
            }

            return new CheckInOutDto
            {
                Id = existing.Id,
                ReservaId = existing.ReservaId,
                FechaCheckIn = existing.FechaCheckIn,
                FechaCheckOut = existing.FechaCheckOut,
                Observaciones = existing.Observaciones
            };
        }

        public async Task<IEnumerable<CheckInOutDto>> GetAllAsync()
        {
            var list = await _checkRepo.GetAllAsync();

            return list.Select(ent => new CheckInOutDto
            {
                Id = ent.Id,
                ReservaId = ent.ReservaId,
                FechaCheckIn = ent.FechaCheckIn,
                FechaCheckOut = ent.FechaCheckOut,
                Observaciones = ent.Observaciones
            });
        }

        public async Task<CheckInOutDto?> GetByIdAsync(Guid id)
        {
            var ent = await _checkRepo.GetByIdAsync(id);
            if (ent == null) return null;

            return new CheckInOutDto
            {
                Id = ent.Id,
                ReservaId = ent.ReservaId,
                FechaCheckIn = ent.FechaCheckIn,
                FechaCheckOut = ent.FechaCheckOut,
                Observaciones = ent.Observaciones
            };
        }

        public async Task<CheckInOutDto?> UpdateAsync(Guid id, UpdateCheckInOutRequest request)
        {
            var ent = await _checkRepo.GetByIdAsync(id);
            if (ent == null) return null;

            ent.FechaCheckIn = request.FechaCheckIn;
            ent.FechaCheckOut = request.FechaCheckOut;
            ent.Observaciones = request.Observaciones;

            var updated = await _checkRepo.UpdateAsync(ent);

            return new CheckInOutDto
            {
                Id = updated.Id,
                ReservaId = updated.ReservaId,
                FechaCheckIn = updated.FechaCheckIn,
                FechaCheckOut = updated.FechaCheckOut,
                Observaciones = updated.Observaciones
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ent = await _checkRepo.GetByIdAsync(id);
            if (ent == null) return false;

            await _checkRepo.DeleteAsync(id);
            return true;
        }
    }
}