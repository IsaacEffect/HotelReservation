<<<<<<< HEAD
namespace HotelReservation.Application.Dtos
=======
﻿namespace HotelReservation.Application.Dtos
>>>>>>> origin/develop
{
    public record ObtenerCategoriaDto
    {
        public Guid Id { get; set; }
<<<<<<< HEAD
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
=======
        public string NombreCategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public decimal PrecioPorNoche { get; set; }
>>>>>>> origin/develop
    }
}