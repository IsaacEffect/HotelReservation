using System;
namespace HotelReservation.Domain.Entities
{
    public class RoomCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;


        public RoomCategory(string name, string description = "")
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }

        // For EF
        protected RoomCategory() { }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre inválido");
            Name = name;
        }
    }
}
