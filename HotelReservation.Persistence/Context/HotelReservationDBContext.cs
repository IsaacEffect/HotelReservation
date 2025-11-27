using HotelReservation.Domain.Entities;
using HotelReservation.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Context
{
    public class HotelReservationDBContext(DbContextOptions<HotelReservationDBContext> options) : DbContext(options)
    {
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CategoriaHabitacion> CategoriasHabitacion { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFactura { get; set; }
        public DbSet<CheckInOut> CheckInOut { get; set; }
        public DbSet<HistorialReserva> HistorialReservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());

            //-----------------------

            modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.Estado);
            modelBuilder.Entity<Cliente>().HasQueryFilter(c => c.Estado);

            //-----------------------

            modelBuilder.Entity<CheckInOut>(entity =>
            {
                entity.ToTable("CheckInOut");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Observaciones).HasMaxLength(255);
                entity.HasIndex(e => e.ReservaId).IsUnique(false);
                entity.HasOne(e => e.Reserva)
                      .WithOne()
                      .HasForeignKey<CheckInOut>(e => e.ReservaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HistorialReserva>(entity =>
            {
                entity.ToTable("HistorialReservas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Motivo).HasMaxLength(100);
            });

            modelBuilder.Entity<DetalleFactura>()
                        .ToTable("DetalleFactura");

            base.OnModelCreating(modelBuilder);
        }
    }
}
