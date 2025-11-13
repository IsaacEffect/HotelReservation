using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos
{
    public record ActualizarClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        public string? Telefono { get; set; }

        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "La cédula debe tener el formato 000-0000000-0.")]
        [StringLength(20, ErrorMessage = "El documento de identidad no debe superar los 20 caracteres.")]
        public string? DocumentoIdentidad { get; set; }
    }
}
