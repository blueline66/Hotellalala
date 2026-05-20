namespace HotelBooking.Application.Pricing;

public class PremiumDiscountStrategy : IPriceStrategy
{
    public decimal Calculate(decimal basePrice, int nights)
    {
        decimal total = basePrice * nights;

        if (nights >= 7)
        {
            total *= 0.8m;
        }

        return total;
    }
}