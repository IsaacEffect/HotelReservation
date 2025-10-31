using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Persistence.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.IdCliente);

            builder.Property(c => c.IdCliente)
                .HasColumnName("Id")
                .HasDefaultValueSql("NEWID()");

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Correo)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(c => c.Telefono)
                .HasMaxLength(50);

            builder.Property(c => c.DocumentoIdentidad)
                .HasMaxLength(50);

            builder.Property(c => c.Estado)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
