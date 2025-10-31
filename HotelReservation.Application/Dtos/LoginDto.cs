using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos
{
    public record LoginDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contraseña { get; set; } = string.Empty;
    }
}
