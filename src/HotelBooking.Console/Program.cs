using HotelBooking.Application;
using HotelBooking.Infrastructure;

// Налаштування (Dependency Injection "на колінці")
var roomRepo = new InMemoryRoomRepository();
var reservationRepo = new InMemoryReservationRepository();
var bookingService = new BookingService(roomRepo, reservationRepo);

Console.WriteLine("=== Система бронювання готелю ===");
Console.WriteLine("Доступні номери:");

var rooms = roomRepo.GetAll().ToList();
foreach (var room in rooms)
{
    Console.WriteLine($"- Номер {room.Number} (ID: {room.Id}) | Ціна за добу: {room.BasePrice}");
}

Console.WriteLine("\nСпробуємо забронювати перший номер...");
var roomToBook = rooms.First();

// Імітація введення даних
string guest = "Іван Іванов";
DateTime checkIn = DateTime.Now.AddDays(1);
DateTime checkOut = DateTime.Now.AddDays(5);

Console.WriteLine($"Гість: {guest}, Заїзд: {checkIn.ToShortDateString()}, Виїзд: {checkOut.ToShortDateString()}");

// Викликаємо сервіс (Вертикальний зріз)
bool isSuccess = bookingService.TryBookRoom(roomToBook.Id, guest, checkIn, checkOut, out string message);
Console.WriteLine(message);

// Спробуємо забронювати на ті ж дати ще раз (має бути помилка)
Console.WriteLine("\nСпроба подвійного бронювання:");
bookingService.TryBookRoom(roomToBook.Id, "Петро Петров", checkIn, checkOut, out string errorMessage);
Console.WriteLine(errorMessage);

Console.ReadLine();