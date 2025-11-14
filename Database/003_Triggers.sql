USE HotelReservationDB;
GO

-- Calcula el monto total de la reserva (días * precio por noche) automáticamente.

CREATE TRIGGER TRG_CalcularTotalReserva
ON Reservas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE r
    SET r.Total = 
        DATEDIFF(DAY, i.FechaInicio, i.FechaFin) * c.PrecioPorNoche
    FROM Reservas r
    INNER JOIN inserted i ON r.Id = i.Id
    INNER JOIN Habitaciones h ON i.HabitacionId = h.Id
    INNER JOIN CategoriasHabitacion c ON h.CategoriaId = c.Id;
END;
GO