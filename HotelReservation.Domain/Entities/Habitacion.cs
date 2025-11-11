namespace HotelReservation.Domain.Entities
{
    public enum HabitacionStatus
    {
        Available,
        Occupied,
        Maintenance
    }

    public class Habitacion
    {
        public Guid Id { get; private set; }
        public int Number { get; private set; }
        public Guid CategoryId { get; private set; }
        public HabitacionStatus Status { get; private set; }
        public decimal Price { get; private set; }

        public Habitacion(int number, Guid categoryId, decimal price)
        {
            Id = Guid.NewGuid();
            Number = number;
            CategoryId = categoryId;
            Price = price;
            Status = HabitacionStatus.Available;
        }

        // For EF
        protected Habitacion() { }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new ArgumentException("Precio no puede ser negativo");
            Price = newPrice;
        }

        public void ChangeStatus(HabitacionStatus newStatus) => Status = newStatus;

        public void UpdateNumber(int newNumber)
        {
            if (newNumber <= 0) throw new ArgumentException("Número inválido");
            Number = newNumber;
        }
    }
}