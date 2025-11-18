using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace HotelReservation.Persistence.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly string _connectionString;

        public ReservaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HotelDBConnection")!;
        }

        // ================================================
        // DISPONIBILIDAD DE HABITACIÓN
        // ================================================

        public async Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await HabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin, null);
        }

        public async Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaIdExcluir)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
                SELECT COUNT(1)
                FROM Reservas r
                WHERE r.HabitacionId = @habitacionId
                  AND r.EstadoReserva NOT IN ('Cancelada', 'Completada')
                  AND (r.FechaInicio < @fechaFin AND r.FechaFin > @fechaInicio)
                  AND (@reservaIdExcluir IS NULL OR r.Id != @reservaIdExcluir)";

            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@habitacionId", habitacionId);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
            cmd.Parameters.AddWithValue("@reservaIdExcluir", (object?)reservaIdExcluir ?? DBNull.Value);

            int count = (int)await cmd.ExecuteScalarAsync();
            return count == 0;
        }

        // ================================================
        // CRUD
        // ================================================

        public async Task<Guid> CrearReservaAsync(Reserva reserva)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                INSERT INTO Reservas 
                (FechaInicio, FechaFin, EstadoReserva, ClienteId, HabitacionId, UsuarioId, FechaReserva)
                OUTPUT INSERTED.Id
                VALUES (@FechaInicio, @FechaFin, @EstadoReserva, @ClienteId, @HabitacionId, @UsuarioId, @FechaReserva)",
                conn);

            cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
            cmd.Parameters.AddWithValue("@EstadoReserva", reserva.EstadoReserva);
            cmd.Parameters.AddWithValue("@ClienteId", reserva.ClienteId);
            cmd.Parameters.AddWithValue("@HabitacionId", reserva.HabitacionId);
            cmd.Parameters.AddWithValue("@UsuarioId", reserva.UsuarioId);
            cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);

            return (Guid)await cmd.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<Reserva>> GetReservasAsync()
        {
            var reservas = new List<Reserva>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Reservas ORDER BY FechaInicio DESC", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reservas.Add(new Reserva
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    FechaReserva = reader.GetDateTime(reader.GetOrdinal("FechaReserva")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    EstadoReserva = reader.GetString(reader.GetOrdinal("EstadoReserva")),
                    ClienteId = reader.GetGuid(reader.GetOrdinal("ClienteId")),
                    HabitacionId = reader.GetGuid(reader.GetOrdinal("HabitacionId")),
                    UsuarioId = reader.GetGuid(reader.GetOrdinal("UsuarioId")),
                    Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? null : reader.GetDecimal(reader.GetOrdinal("Total"))
                });
            }

            return reservas;
        }

        public async Task<Reserva?> ObtenerReservaPorIdAsync(Guid id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Reservas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new Reserva
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FechaReserva = reader.GetDateTime(reader.GetOrdinal("FechaReserva")),
                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                EstadoReserva = reader.GetString(reader.GetOrdinal("EstadoReserva")),
                ClienteId = reader.GetGuid(reader.GetOrdinal("ClienteId")),
                HabitacionId = reader.GetGuid(reader.GetOrdinal("HabitacionId")),
                UsuarioId = reader.GetGuid(reader.GetOrdinal("UsuarioId")),
                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? null : reader.GetDecimal(reader.GetOrdinal("Total"))
            };
        }

        public async Task ModificarReservaAsync(Reserva entity)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                UPDATE Reservas
                SET FechaInicio = @FechaInicio,
                    FechaFin = @FechaFin,
                    EstadoReserva = @EstadoReserva,
                    HabitacionId = @HabitacionId
                WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@FechaInicio", entity.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", entity.FechaFin);
            cmd.Parameters.AddWithValue("@EstadoReserva", entity.EstadoReserva);
            cmd.Parameters.AddWithValue("@HabitacionId", entity.HabitacionId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CancelarReservaAsync(Guid id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "UPDATE Reservas SET EstadoReserva = 'Cancelada' WHERE Id = @Id",
                conn);

            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ================================================
        // CONSULTAS CON DETALLES (JOIN)
        // ================================================

        public async Task<IEnumerable<object>> ObtenerReservasConDetallesAsync()
        {
            var reservas = new List<object>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
                SELECT 
                    r.Id, r.FechaReserva, r.FechaInicio, r.FechaFin,
                    r.EstadoReserva, r.Total,
                    c.Nombre + ' ' + c.Apellido AS NombreCliente,
                    c.Correo AS CorreoCliente,
                    h.Numero AS NumeroHabitacion,
                    cat.NombreCategoria AS Categoria,
                    cat.PrecioPorNoche,
                    u.Nombre + ' ' + u.Apellido AS NombreUsuario
                FROM Reservas r
                INNER JOIN Clientes c ON r.ClienteId = c.Id
                INNER JOIN Habitaciones h ON r.HabitacionId = h.Id
                INNER JOIN CategoriasHabitacion cat ON h.CategoriaId = cat.Id
                INNER JOIN Usuarios u ON r.UsuarioId = u.Id
                ORDER BY r.FechaInicio DESC";

            var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reservas.Add(new
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    FechaReserva = reader.GetDateTime(reader.GetOrdinal("FechaReserva")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    EstadoReserva = reader.GetString(reader.GetOrdinal("EstadoReserva")),
                    NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                    CorreoCliente = reader.GetString(reader.GetOrdinal("CorreoCliente")),
                    NumeroHabitacion = reader.GetString(reader.GetOrdinal("NumeroHabitacion")),
                    Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
                    PrecioPorNoche = reader.GetDecimal(reader.GetOrdinal("PrecioPorNoche")),
                    NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                });
            }

            return reservas;
        }

        // ================================================
        // CONSULTAS POR ESTADO
        // ================================================

        public async Task<IEnumerable<Reserva>> ObtenerReservasPorEstadoAsync(string estado)
        {
            var reservas = new List<Reserva>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "SELECT * FROM Reservas WHERE EstadoReserva = @Estado",
                conn);

            cmd.Parameters.AddWithValue("@Estado", estado);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reservas.Add(new Reserva
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    FechaReserva = reader.GetDateTime(reader.GetOrdinal("FechaReserva")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    EstadoReserva = reader.GetString(reader.GetOrdinal("EstadoReserva")),
                    ClienteId = reader.GetGuid(reader.GetOrdinal("ClienteId")),
                    HabitacionId = reader.GetGuid(reader.GetOrdinal("HabitacionId")),
                    UsuarioId = reader.GetGuid(reader.GetOrdinal("UsuarioId")),
                    Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? null : reader.GetDecimal(reader.GetOrdinal("Total"))
                });
            }

            return reservas;
        }
        
        // --- MÉTODOS GENÉRICOS (STUBS) ---
        
        public Task<Reserva?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException("Use ObtenerReservaPorIdAsync");
        }
        
        public Task<IEnumerable<Reserva>> GetAllAsync()
        {
            throw new NotImplementedException("Use ObtenerReservasAsync");
        }
        
        public Task<Reserva> AddAsync(Reserva entity)
        {
            throw new NotImplementedException("Use CrearReservaAsync");
        }
        
        public Task<Reserva> UpdateAsync(Reserva entity)
        {
            throw new NotImplementedException("Use ModificarReservaAsync");
        }
        
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException("Use CancelarReservaAsync");
        }

        public Task<IEnumerable<Reserva>> ObtenerReservasAsync()
        {
            throw new NotImplementedException();
        }

        Task IReservaRepository.GetByIdAsync(Guid reservaId)
        {
            return GetByIdAsync(reservaId);
        }
    }
}