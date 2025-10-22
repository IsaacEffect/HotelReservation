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

            builder.HasKey(r => r.RolId);

            builder.Property(r => r.RolId)
                .HasColumnName("Id")
                .HasDefaultValueSql("NEWID()");

            builder.Property(r => r.NombreRol)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("NombreRol");
        }
    }
}
