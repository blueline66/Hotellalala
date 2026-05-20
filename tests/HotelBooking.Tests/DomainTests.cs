using Xunit;
using HotelBooking.Domain;
using HotelBooking.Application;
using HotelBooking.Infrastructure;

namespace HotelBooking.Tests;

public class DomainTests
{
    // Тест 1: Перевірка правильного розрахунку ціни стандартного номера
    [Fact]
    public void StandardRoom_CalculatePrice_ReturnsCorrectAmount()
    {
        var room = new StandardRoom("101", 1000m);
        var price = room.CalculatePrice(3); // 3 дні
        Assert.Equal(3000m, price);
    }

    // Тест 2: Перевірка розрахунку ціни преміум номера (з комісією)
    [Fact]
    public void PremiumRoom_CalculatePrice_ReturnsCorrectAmountWithFee()
    {
        var room = new PremiumRoom("201", 2000m, 500m);
        var price = room.CalculatePrice(2); // 2 дні: (2000 * 2) + 500
        Assert.Equal(4500m, price); 
    }

    // Тест 3: Клас DateRange викидає помилку, якщо дата виїзду раніше застави
    [Fact]
    public void DateRange_EndBeforeStart_ThrowsException()
    {
        var start = DateTime.Now;
        var end = start.AddDays(-1);
        Assert.Throws<ArgumentException>(() => new DateRange(start, end));
    }

    // Тест 4: Перевірка перетину дат (щоб не було накладок)
    [Fact]
    public void DateRange_Overlaps_ReturnsTrueWhenDatesIntersect()
    {
        var range1 = new DateRange(DateTime.Now, DateTime.Now.AddDays(5));
        var range2 = new DateRange(DateTime.Now.AddDays(3), DateTime.Now.AddDays(7));
        Assert.True(range1.Overlaps(range2));
    }

    // Тест 5: Сервіс бронювання не дозволяє подвійне бронювання
    [Fact]
    public void BookingService_DoubleBooking_ReturnsFalse()
    {
        // Налаштовуємо базу в пам'яті
        var roomRepo = new InMemoryRoomRepository();
        var resRepo = new InMemoryReservationRepository();
        var service = new BookingService(roomRepo, resRepo);
        
        var room = roomRepo.GetAll().First();
        var start = DateTime.Now.AddDays(1);
        var end = DateTime.Now.AddDays(5);

        // Перше бронювання (успішне)
        service.TryBookRoom(room.Id, "Guest 1", start, end, out _);
        
        // Друге бронювання на ті ж самі дати (має бути помилка)
        bool isSuccess = service.TryBookRoom(room.Id, "Guest 2", start, end, out string message);

        Assert.False(isSuccess);
        Assert.Contains("вже зайнятий", message);
    }
}