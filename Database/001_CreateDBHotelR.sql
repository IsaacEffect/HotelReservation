-- CREACIÓN DE BASE DE DATOS

CREATE DATABASE HotelReservationDB;
GO
USE HotelReservationDB;
GO

-- ROLES Y USUARIOS

CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    NombreRol NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Usuarios (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(120) NOT NULL UNIQUE,
    Contraseña NVARCHAR(255) NOT NULL,
    RolId UNIQUEIDENTIFIER NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (RolId) REFERENCES Roles(Id) ON DELETE CASCADE
);
GO

-- CLIENTES

CREATE TABLE Clientes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(120) NOT NULL,
    Telefono NVARCHAR(50),
    DocumentoIdentidad NVARCHAR(50),
    FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- CATEGORÍAS Y HABITACIONES

CREATE TABLE CategoriasHabitacion (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    NombreCategoria NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Capacidad INT NOT NULL CHECK (Capacidad > 0),
    PrecioPorNoche DECIMAL(10,2) NOT NULL CHECK (PrecioPorNoche >= 0)
);
GO

CREATE TABLE Habitaciones (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Numero NVARCHAR(20) NOT NULL UNIQUE,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Disponible' CHECK (Estado IN ('Disponible', 'Ocupada', 'Mantenimiento')),
    CategoriaId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_Habitaciones_CategoriasHabitacion FOREIGN KEY (CategoriaId) REFERENCES CategoriasHabitacion(Id) ON DELETE CASCADE
);
GO

-- RESERVAS Y DETALLES

CREATE TABLE Reservas (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaReserva DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    EstadoReserva NVARCHAR(50) NOT NULL DEFAULT 'Activa'
        CHECK (EstadoReserva IN ('Activa', 'Pendiente', 'Confirmada', 'Cancelada', 'Completada')),
    ClienteId UNIQUEIDENTIFIER NOT NULL,
    HabitacionId UNIQUEIDENTIFIER NOT NULL,
    UsuarioId UNIQUEIDENTIFIER NOT NULL,
    Total DECIMAL(10,2) NULL CHECK (Total >= 0),
    CONSTRAINT FK_Reservas_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reservas_Habitaciones FOREIGN KEY (HabitacionId) REFERENCES Habitaciones(Id),
    CONSTRAINT FK_Reservas_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);
GO

CREATE TABLE DetalleReserva (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ReservaId UNIQUEIDENTIFIER NOT NULL,
    Descripcion NVARCHAR(200),
    Cantidad INT NOT NULL DEFAULT 1 CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (PrecioUnitario >= 0),
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT FK_DetalleReserva_Reservas FOREIGN KEY (ReservaId) REFERENCES Reservas(Id) ON DELETE CASCADE
);
GO

-- FACTURACIÓN

CREATE TABLE Facturas (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ReservaId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    FechaEmision DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    MontoTotal DECIMAL(10,2) NOT NULL CHECK (MontoTotal >= 0),
    MetodoPago NVARCHAR(50) NOT NULL CHECK (MetodoPago IN ('Efectivo', 'Tarjeta', 'Transferencia')),
    CONSTRAINT FK_Facturas_Reservas FOREIGN KEY (ReservaId) REFERENCES Reservas(Id) ON DELETE CASCADE
);
GO

CREATE TABLE DetalleFactura (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FacturaId UNIQUEIDENTIFIER NOT NULL,
    Descripcion NVARCHAR(200),
    Cantidad INT NOT NULL DEFAULT 1 CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario >= 0),
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT FK_DetalleFactura_Facturas FOREIGN KEY (FacturaId) REFERENCES Facturas(Id) ON DELETE CASCADE
);
GO

-- CHECK-IN / CHECK-OUT E HISTORIAL

CREATE TABLE CheckInOut (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ReservaId UNIQUEIDENTIFIER NOT NULL,
    FechaCheckIn DATETIME2 NULL,
    FechaCheckOut DATETIME2 NULL,
    Observaciones NVARCHAR(255),
    CONSTRAINT FK_CheckInOut_Reservas FOREIGN KEY (ReservaId) REFERENCES Reservas(Id) ON DELETE CASCADE
);
GO

CREATE TABLE HistorialReservas (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    HabitacionId UNIQUEIDENTIFIER NOT NULL,
    ClienteId UNIQUEIDENTIFIER NOT NULL,
    FechaEntrada DATETIME2 NOT NULL,
    FechaSalida DATETIME2 NOT NULL,
    Motivo NVARCHAR(100),
    CONSTRAINT FK_HistorialReservas_Habitaciones FOREIGN KEY (HabitacionId) REFERENCES Habitaciones(Id),
    CONSTRAINT FK_HistorialReservas_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
GO

-- ÍNDICES

CREATE INDEX IX_Reservas_ClienteId ON Reservas(ClienteId);
CREATE INDEX IX_Reservas_HabitacionId ON Reservas(HabitacionId);
CREATE INDEX IX_Habitaciones_Estado ON Habitaciones(Estado);
CREATE INDEX IX_Facturas_FechaEmision ON Facturas(FechaEmision);
GO