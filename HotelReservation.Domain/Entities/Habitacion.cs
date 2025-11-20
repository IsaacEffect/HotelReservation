namespace HotelReservation.Domain.Entities
{
    public class Habitacion
    {
        public Guid Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string? Estado { get; set; } // Valores: 'Disponible', 'Ocupada', 'Mantenimiento'
        public Guid CategoriaId { get; set; }

        // --- Propiedad de Navegación ---
        public virtual CategoriaHabitacion? Categoria { get; set; }
    }
}