namespace HotelBooking.Domain;

public class PremiumRoom : Room
{
    public decimal ExtraFee { get; }

    public PremiumRoom(string number, decimal basePrice, decimal extraFee) : base(number, basePrice)
    {
        ExtraFee = extraFee;
    }

    public override decimal CalculatePrice(int days) => (BasePrice * days) + ExtraFee;
}