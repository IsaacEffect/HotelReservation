CREATE DATABASE Reserva_Hoteles_;
USE Reserva_Hoteles_;

CREATE TABLE Clientes (
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Nombre NVARCHAR(100) NOT NULL,
  Apellido NVARCHAR(100) NOT NULL,
  Correo NVARCHAR(200) NOT NULL
);

CREATE TABLE CategoriasHabitacion (
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Nombre NVARCHAR(100) NOT NULL,
  Descripcion NVARCHAR(250) NULL,
  PrecioBase DECIMAL(10,2) NOT NULL
);

CREATE TABLE Habitaciones (
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Numero NVARCHAR(20) NOT NULL,
  CategoriaId UNIQUEIDENTIFIER NOT NULL,
  Estado NVARCHAR(20) NOT NULL, -- Disponible, Ocupada, Mantenimiento
  CONSTRAINT FK_Habitacion_Categoria FOREIGN KEY(CategoriaId) REFERENCES CategoriasHabitacion(Id)
);

CREATE TABLE Reservas (
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ClienteId UNIQUEIDENTIFIER NOT NULL,
  HabitacionId UNIQUEIDENTIFIER NOT NULL,
  FechaEntrada DATETIME2 NOT NULL,
  FechaSalida DATETIME2 NOT NULL,
  Estado NVARCHAR(20) NOT NULL, -- Pendiente, Confirmada, Cancelada, Completada
  Total DECIMAL(10,2) NOT NULL,
  FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  CONSTRAINT FK_Reserva_Cliente FOREIGN KEY(ClienteId) REFERENCES Clientes(Id),
  CONSTRAINT FK_Reserva_Habitacion FOREIGN KEY(HabitacionId) REFERENCES Habitaciones(Id)
);


