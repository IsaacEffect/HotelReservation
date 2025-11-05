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

-- Renombrar la columna 'Contraseña' a 'Contrasena' para evitar problemas con caracteres especiales
EXEC sp_rename 'Usuarios.Contraseña', 'Contrasena', 'COLUMN';
GO
