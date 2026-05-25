namespace HotelBooking.Domain;

public class PremiumRoom : Room
{
    public decimal ExtraCharge { get; }
    public decimal ExtraFee { get; }

    public PremiumRoom(string number, decimal basePrice, decimal extraCharge) : base(number, basePrice)
    {
        ExtraCharge = extraCharge;
    }

    public override decimal CalculatePrice(int days) => (BasePrice * days) + ExtraCharge;
}