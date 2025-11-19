using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Application.Dtos
{
    public class ObtenerHabitacionDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string? Estado { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
