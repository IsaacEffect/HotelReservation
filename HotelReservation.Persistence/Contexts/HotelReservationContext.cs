using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities; // Aquí estarán tus POCOs del dominio

namespace HotelReservation.Persistence.Contexts
{
    public class HotelReservationContext : DbContext
    {
        public HotelReservationContext(DbContextOptions<HotelReservationContext> options)
            : base(options) { }

        // DbSets = Tablas
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CategoriaHabitacion> CategoriasHabitacion { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<DetalleReserva> DetalleReserva { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<CheckInOut> CheckInOut { get; set; }
        public DbSet<HistorialReserva> HistorialReservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aquí configuraremos las relaciones, restricciones e índices
            // (lo haremos en un paso posterior con Fluent API).
            modelBuilder.Entity<DetalleFactura>()
                .ToTable("DetalleFactura")
                .Property(d => d.Subtotal)
                .HasComputedColumnSql("[Cantidad] * [PrecioUnitario]");

            base.OnModelCreating(modelBuilder);
        }
    }
}