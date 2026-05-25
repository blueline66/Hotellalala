namespace HotelBooking.Application.Pricing;

public interface IPriceStrategy
{
    decimal Calculate(decimal basePrice, int days);
}