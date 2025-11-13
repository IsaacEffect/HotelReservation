using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos
{
    public record InsertarCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
    }
}