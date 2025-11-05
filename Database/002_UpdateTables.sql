USE HotelReservationDB;
GO

-- Agregar campo Estado a la tabla Clientes
ALTER TABLE Clientes
ADD Estado BIT NOT NULL DEFAULT 1;
GO

-- Agregar campo Estado a la tabla Usuarios
ALTER TABLE Usuarios
ADD Estado BIT NOT NULL DEFAULT 1;
GO
