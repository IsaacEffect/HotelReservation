using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Persistence.Context
{
    public class HotelReservationDBContext(DbContextOptions<HotelReservationDBContext> options) : DbContext(options)
    {
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        }
    }
}
