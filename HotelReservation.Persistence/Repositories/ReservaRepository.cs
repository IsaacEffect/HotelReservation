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

        public Task<bool> HabitacionDisponibleAsync(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            return Task.FromResult(HabitacionDisponible(habitacionId, fechaInicio, fechaFin));
        }
        
        public bool HabitacionDisponible(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT COUNT(1) FROM Reservas r
                WHERE r.HabitacionId = @habitacionId
                AND r.EstadoReserva <> 'Cancelada'
                AND NOT (r.FechaFin <= @fechaInicio OR r.FechaInicio >= @fechaFin)", conn); // Nota: Cambié < y > por <= y >= para evitar solapamiento en el límite
            
            cmd.Parameters.AddWithValue("@habitacionId", habitacionId);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

            int count = (int)cmd.ExecuteScalar();
            return count == 0;
        }

        public Task<Guid> CrearReservaAsync(Reserva reserva)
        {
            return Task.FromResult(CrearReserva(reserva));
        }

        public Guid CrearReserva(Reserva reserva)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Reservas 
                (FechaInicio, FechaFin, EstadoReserva, ClienteId, HabitacionId, UsuarioId, Total, FechaReserva)
                OUTPUT INSERTED.Id
                VALUES (@FechaInicio, @FechaFin, @EstadoReserva, @ClienteId, @HabitacionId, @UsuarioId, @Total, @FechaReserva)", conn);

            cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
            cmd.Parameters.AddWithValue("@EstadoReserva", reserva.EstadoReserva);
            cmd.Parameters.AddWithValue("@ClienteId", reserva.ClienteId);
            cmd.Parameters.AddWithValue("@HabitacionId", reserva.HabitacionId);
            cmd.Parameters.AddWithValue("@UsuarioId", reserva.UsuarioId);
            cmd.Parameters.AddWithValue("@Total", reserva.Total);
            cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva); // Asegurando que se guarda la fecha de reserva

            return (Guid)cmd.ExecuteScalar();
        }

        public Task<IEnumerable<Reserva>> ObtenerReservasAsync()
        {
            return Task.FromResult(ObtenerReservas());
        }

        public IEnumerable<Reserva> ObtenerReservas()
        {
            var reservas = new List<Reserva>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM Reservas", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reservas.Add(new Reserva
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    FechaReserva = reader.GetDateTime(reader.GetOrdinal("FechaReserva")), // Añadir si existe en DB
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    EstadoReserva = reader.GetString(reader.GetOrdinal("EstadoReserva")),
                    ClienteId = reader.GetGuid(reader.GetOrdinal("ClienteId")),
                    HabitacionId = reader.GetGuid(reader.GetOrdinal("HabitacionId")),
                    UsuarioId = reader.GetGuid(reader.GetOrdinal("UsuarioId")),
                    Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                });
            }

            return reservas;
        }

        public Task<IEnumerable<object>> ObtenerReservasConDetallesAsync()
        {
            return Task.FromResult(ObtenerReservasConDetalles());
        }

        public IEnumerable<object> ObtenerReservasConDetalles()
        {
            var reservas = new List<object>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var query = @"
                SELECT 
                    r.Id,
                    c.Nombre AS NombreCliente,
                    h.Numero AS NumeroHabitacion,
                    u.Nombre AS NombreUsuario,
                    r.FechaInicio,
                    r.FechaFin,
                    r.EstadoReserva,
                    r.Total
                FROM Reservas r
                INNER JOIN Clientes c ON r.ClienteId = c.Id
                INNER JOIN Habitaciones h ON r.HabitacionId = h.Id
                INNER JOIN Usuarios u ON r.UsuarioId = u.Id
                ORDER BY r.FechaInicio DESC";

            var cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reservas.Add(new
                {
                    Id = reader["Id"],
                    NombreCliente = reader["NombreCliente"].ToString(),
                    NumeroHabitacion = reader["NumeroHabitacion"].ToString(),
                    NombreUsuario = reader["NombreUsuario"].ToString(),
                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                    FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                    EstadoReserva = reader["EstadoReserva"].ToString(),
                    Total = Convert.ToDecimal(reader["Total"])
                });
            }

            return reservas;
        }

        // ----------------------------------------------------------------------
        
        public Task<Reserva?> GetByIdAsync(Guid id) => throw new NotImplementedException("Método no implementado en ReservaRepository. Use métodos específicos.");
        public Task<IEnumerable<Reserva>> GetAllAsync() => throw new NotImplementedException("Método no implementado en ReservaRepository. Use ObtenerReservasAsync.");
        public Task<Reserva> AddAsync(Reserva entity) => throw new NotImplementedException("Método no implementado en ReservaRepository. Use CrearReservaAsync.");
        public Task<Reserva> UpdateAsync(Reserva entity) => throw new NotImplementedException("Método no implementado.");
        public Task DeleteAsync(Guid id) => Task.CompletedTask;
    }
}