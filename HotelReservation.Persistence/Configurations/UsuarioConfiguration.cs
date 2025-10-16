using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.Property(c => c.IdUsuario).HasColumnName("Id");
            builder.Property(c => c.Nombre).HasColumnName("Nombre");
            builder.Property(c => c.Apellido).HasColumnName("Apellido");
            builder.Property(c => c.Correo).HasColumnName("Correo");
            builder.Property(c => c.Contraseña).HasColumnName("Contraseña");

            builder.HasKey(u => u.IdUsuario);

            builder.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(120);

            builder.HasIndex(u => u.Correo)
                .IsUnique();

            builder.Property(u => u.Contraseña)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Estado)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Cascade);
        }    
    }
}

