namespace HotelBooking.Domain;

public class StandardRoom : Room
{
    public StandardRoom(string number, decimal basePrice) : base(number, basePrice) { }

    public override decimal CalculatePrice(int days) => BasePrice * days;
}