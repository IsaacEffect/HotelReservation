using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos
{
    public record InsertarRolDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del rol no debe superar los 50 caracteres.")]
        public string NombreRol { get; set; } = string.Empty;
    }
}
