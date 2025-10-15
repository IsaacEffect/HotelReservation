using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HotelReservation.Domain;
using Microsoft.Extensions.Configuration;

namespace HotelReservation.Persistence.Repositories
{
public class ReservaRepository
{
private readonly string _connectionString;


public ReservaRepository(IConfiguration configuration)
{

    _connectionString = configuration.GetConnectionString("DefaultConnection")!; 
}


        public bool HabitacionDisponible(Guid habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();


            var cmd = new SqlCommand(@"
SELECT COUNT(1) FROM Reservas r
WHERE r.HabitacionId = @habitacionId
AND r.EstadoReserva <> 'Cancelada'
AND NOT (r.FechaFin < @fechaInicio OR r.FechaInicio > @fechaFin)", conn);


            cmd.Parameters.AddWithValue("@habitacionId", habitacionId);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);


            int count = (int)cmd.ExecuteScalar();
            return count == 0;
        }

public Guid CrearReserva(Reserva reserva)
{
using var conn = new SqlConnection(_connectionString);
conn.Open();


var cmd = new SqlCommand
(@"INSERT INTO Reservas (FechaInicio, FechaFin, EstadoReserva, ClienteId, HabitacionId, UsuarioId, Total)
OUTPUT INSERTED.Id
VALUES (@FechaInicio, @FechaFin, @EstadoReserva, @ClienteId, @HabitacionId, @UsuarioId, @Total)", conn);


cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
cmd.Parameters.AddWithValue("@EstadoReserva", reserva.EstadoReserva);
cmd.Parameters.AddWithValue("@ClienteId", reserva.ClienteId);
cmd.Parameters.AddWithValue("@HabitacionId", reserva.HabitacionId);
cmd.Parameters.AddWithValue("@UsuarioId", reserva.UsuarioId);
cmd.Parameters.AddWithValue("@Total", reserva.Total);


return (Guid)cmd.ExecuteScalar();
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
Id = reader.GetGuid("Id"),
FechaInicio = reader.GetDateTime("FechaInicio"),
FechaFin = reader.GetDateTime("FechaFin"),
EstadoReserva = reader.GetString("EstadoReserva"),
ClienteId = reader.GetGuid("ClienteId"),
HabitacionId = reader.GetGuid("HabitacionId"),
UsuarioId = reader.GetGuid("UsuarioId"),
Total = reader.GetDecimal("Total")
});
}
return reservas;
}
}
}

