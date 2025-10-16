using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Persistence.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");

            builder.Property(c => c.IdRol).HasColumnName("Id");
            builder.Property(c => c.NombreRol).HasColumnName("NombreRol");

            builder.HasKey(r => r.IdRol);

            builder.Property(r => r.NombreRol)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
