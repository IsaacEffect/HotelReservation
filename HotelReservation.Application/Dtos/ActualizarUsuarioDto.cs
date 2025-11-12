using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos
{
    public record ActualizarUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public Guid RolId { get; set; }
    }
}
