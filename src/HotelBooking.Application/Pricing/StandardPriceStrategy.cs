namespace HotelBooking.Application.Pricing;

public class StandardPriceStrategy : IPriceStrategy
{
    public decimal Calculate(decimal basePrice, int days)
    {
        return basePrice * days;
    }
}