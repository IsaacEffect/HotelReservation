namespace HotelReservation.Domain.Entities
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance
    }

    public class Room
    {
        public Guid Id { get; private set; }
        public int Number { get; private set; }
        public Guid CategoryId { get; private set; }
        public RoomStatus Status { get; private set; }
        public decimal Price { get; private set; }

        public Room(int number, Guid categoryId, decimal price)
        {
            Id = Guid.NewGuid();
            Number = number;
            CategoryId = categoryId;
            Price = price;
            Status = RoomStatus.Available;
        }

        // For EF
        protected Room() { }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new ArgumentException("Precio no puede ser negativo");
            Price = newPrice;
        }

        public void ChangeStatus(RoomStatus newStatus) => Status = newStatus;

        public void UpdateNumber(int newNumber)
        {
            if (newNumber <= 0) throw new ArgumentException("Número inválido");
            Number = newNumber;
        }
    }
}
