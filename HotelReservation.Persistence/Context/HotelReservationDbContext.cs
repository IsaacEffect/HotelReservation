using Microsoft.EntityFrameworkCore;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Persistence.Context
{
    public class HotelReservationDbContext : DbContext
    {
        public HotelReservationDbContext(DbContextOptions<HotelReservationDbContext> options) : base(options) { }

        public DbSet<CheckInOut> CheckInOuts { get; set; }
        public DbSet<HistorialReserva> HistorialReservas { get; set; }
        public DbSet<Reserva>? Reservas { get; set; }
        public DbSet<Habitacion>? Habitaciones { get; set; }
        public DbSet<Cliente>? Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

        }
    }
}
