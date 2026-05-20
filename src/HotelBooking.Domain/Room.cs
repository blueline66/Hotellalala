namespace HotelBooking.Domain;

public abstract class Room
{
    public Guid Id { get; protected set; }
    public string Number { get; protected set; }
    public decimal BasePrice { get; protected set; }

    protected Room(string number, decimal basePrice)
    {
        Id = Guid.NewGuid();
        Number = number;
        BasePrice = basePrice;
    }

    // Абстрактний метод для поліморфізму
    public abstract decimal CalculatePrice(int days);
}