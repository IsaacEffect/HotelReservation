using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


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

            var scalarCount = await cmd.ExecuteScalarAsync();

            // Safe conversion from object? to int without unboxing null
            int count = scalarCount is int ic
                ? ic
                : scalarCount is long il
                    ? Convert.ToInt32(il)
                    : scalarCount is decimal idc
                        ? Convert.ToInt32(idc)
                        : scalarCount is null || scalarCount == DBNull.Value
                            ? 0
                            : Convert.ToInt32(scalarCount);

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
        (FechaInicio, FechaFin, EstadoReserva, ClienteId, HabitacionId, UsuarioId, FechaReserva, Total)
        OUTPUT INSERTED.Id
        VALUES (
            @FechaInicio, 
            @FechaFin, 
            @EstadoReserva, 
            @ClienteId, 
            @HabitacionId, 
            @UsuarioId, 
            @FechaReserva,
            @TotalCalculado 
        )",
                conn);

            // 1. Obtener el PrecioPorNoche de la Categoría de la Habitación
            var precioQuery = @"
        SELECT cat.PrecioPorNoche
        FROM Habitaciones h
        INNER JOIN CategoriasHabitacion cat ON h.CategoriaId = cat.Id
        WHERE h.Id = @HabitacionId";

            var precioCmd = new SqlCommand(precioQuery, conn);
            precioCmd.Parameters.AddWithValue("@HabitacionId", reserva.HabitacionId);

            object? precioResult = await precioCmd.ExecuteScalarAsync();
            if (precioResult == null || precioResult == DBNull.Value)
            {
                // Manejar el caso de que la habitación o su categoría no exista.
                throw new InvalidOperationException($"No se pudo encontrar el precio por noche para la habitación {reserva.HabitacionId}.");
            }

            // ARREGLO 1: Usar '!' para indicar al compilador que ya se verificó la nulidad.
            decimal precioPorNoche = (decimal)precioResult!;

            // 2. Calcular la diferencia de días (FechaFin - FechaInicio)
            // Se asume que FechaInicio y FechaFin están en la entidad Reserva.
            int dias = (int)(reserva.FechaFin - reserva.FechaInicio).TotalDays;
            if (dias <= 0)
            {
                // Si son menos de un día, podría ser 1 día si la política lo exige, o lanzar un error.
                // Usaremos Math.Max(1, dias) si la política es cobrar al menos un día.
                // Para este ejemplo, si es 0 o menos, lanzamos una excepción simple.
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            // 3. Calcular el Total
            decimal totalCalculado = dias * precioPorNoche;

            // Asignar el total calculado a la entidad antes de la inserción
            reserva.Total = totalCalculado;

            // Añadir todos los parámetros al comando INSERT
            cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
            cmd.Parameters.AddWithValue("@EstadoReserva", reserva.EstadoReserva);
            cmd.Parameters.AddWithValue("@ClienteId", reserva.ClienteId);
            cmd.Parameters.AddWithValue("@HabitacionId", reserva.HabitacionId);
            cmd.Parameters.AddWithValue("@UsuarioId", reserva.UsuarioId);
            cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
            cmd.Parameters.AddWithValue("@TotalCalculado", totalCalculado); // Nuevo parámetro

            // Ejecutar el INSERT y obtener el ID
            // ARREGLO 2: Usar '!' para suprimir el warning, ya que 'OUTPUT INSERTED.Id'
            // garantiza que se devolverá un GUID si el INSERT tiene éxito.
            // Ejecutar el INSERT y obtener el ID de forma segura
            var inserted = await cmd.ExecuteScalarAsync();

            if (inserted == null || inserted == DBNull.Value)
                throw new InvalidOperationException("No se pudo insertar la reserva (Id nulo).");

            if (inserted is Guid newId)
                return newId;

            if (inserted is string s && Guid.TryParse(s, out var parsedId))
                return parsedId;

            // Intentar convertir otros tipos a Guid (por ejemplo byte[] o numeric)
            try
            {
                return Guid.Parse(inserted.ToString()!);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("El Id insertado tiene un formato inválido.", ex);
            }
        }

        public async Task<IEnumerable<Reserva>> ObtenerReservasAsync()
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
                Total = reader.IsDBNull(reader.GetOrdinal("Total"))
            ? 0
            : reader.GetDecimal(reader.GetOrdinal("Total")),
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
                            HabitacionId = @HabitacionId,
                            Total = @Total
                        WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@FechaInicio", entity.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", entity.FechaFin);
            cmd.Parameters.AddWithValue("@EstadoReserva", entity.EstadoReserva);
            cmd.Parameters.AddWithValue("@HabitacionId", entity.HabitacionId);
            cmd.Parameters.AddWithValue("@Total", (object?)entity.Total ?? DBNull.Value);

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

                    // SE AGREGA EL TOTAL AQUÍ
                    Total = reader.IsDBNull(reader.GetOrdinal("Total"))
                        ? 0
                        : reader.GetDecimal(reader.GetOrdinal("Total")),
                });
            }

            return reservas;
        }

        public async Task<decimal> ObtenerPrecioHabitacionAsync(Guid habitacionId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
        SELECT cat.PrecioPorNoche
        FROM Habitaciones h
        INNER JOIN CategoriasHabitacion cat ON h.CategoriaId = cat.Id
        WHERE h.Id = @HabitacionId";

            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@HabitacionId", habitacionId);

            object? result = await cmd.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
                throw new InvalidOperationException("No se pudo obtener el precio por noche.");

            return (decimal)result;
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
    }
}