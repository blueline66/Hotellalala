using HotelBooking.Application;
using HotelBooking.Application.Pricing;
using HotelBooking.Infrastructure;
using HotelBooking.Domain;

// Налаштування
var roomRepo = new InMemoryRoomRepository();

var reservationRepo =
    new JsonReservationRepository("reservations.json");

var bookingService =
    new BookingService(roomRepo, reservationRepo);

Console.WriteLine("=== Система бронювання готелю ===");

bool running = true;

while (running)
{
    Console.WriteLine("\nМеню:");
    Console.WriteLine("1. Показати номери");
    Console.WriteLine("2. Створити бронювання");
    Console.WriteLine("3. Показати бронювання");
    Console.WriteLine("4. Strategy Pattern");
    Console.WriteLine("5. LINQ-аналітика");
    Console.WriteLine("0. Вихід");

    Console.Write("\nВаш вибір: ");

    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":

            Console.WriteLine("\nДоступні номери:");

            var rooms = roomRepo.GetAll().ToList();

            foreach (var room in rooms)
            {
                Console.WriteLine($"Номер: {room.Number}");
                Console.WriteLine($"ID: {room.Id}");
                Console.WriteLine($"Ціна: {room.BasePrice}");
                Console.WriteLine("-------------------");
            }

            break;

        case "2":

            var availableRooms =
                roomRepo.GetAll().ToList();

            Console.WriteLine("\nОберіть кімнату:");

            foreach (var room in availableRooms)
            {
                Console.WriteLine(
                    $"{room.Id} | Номер {room.Number}");
            }

            Console.Write("\nВведіть ID кімнати: ");

            Guid roomId =
                Guid.Parse(Console.ReadLine()!);

            Console.Write("Ім'я гостя: ");

            string guest =
                Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(guest))
            {
                Console.WriteLine(
                    "Ім'я не може бути порожнім!");

                break;
            }

            Console.Write(
                "Дата заїзду (yyyy-mm-dd): ");

            DateTime checkIn =
                DateTime.Parse(Console.ReadLine()!);

            Console.Write(
                "Дата виїзду (yyyy-mm-dd): ");

            DateTime checkOut =
                DateTime.Parse(Console.ReadLine()!);

            bool isSuccess =
                bookingService.TryBookRoom(
                    roomId,
                    guest,
                    checkIn,
                    checkOut,
                    out string message);

            Console.WriteLine(message);

            break;

        case "3":

            var reservations =
                reservationRepo.GetAll().ToList();

            Console.WriteLine("\nСписок бронювань:");

            if (!reservations.Any())
            {
                Console.WriteLine(
                    "Бронювань немає.");

                break;
            }

            foreach (var reservation in reservations)
            {
                var room =
                    roomRepo.GetAll()
                        .FirstOrDefault(r =>
                            r.Id == reservation.RoomId);

                Console.WriteLine(
                    $"Гість: {reservation.GuestName}");

                Console.WriteLine(
                    $"Номер кімнати: {room?.Number}");

                Console.WriteLine(
                    $"Заїзд: {reservation.Dates.Start.ToShortDateString()}");

                Console.WriteLine(
                    $"Виїзд: {reservation.Dates.End.ToShortDateString()}");

                Console.WriteLine(
                    $"Статус: {reservation.Status}");

                Console.WriteLine("-------------------");
            }

            break;

        case "4":

            Console.WriteLine(
                "\n=== Strategy Pattern ===");

            IPriceStrategy premiumStrategy =
                new PremiumDiscountStrategy();

            decimal premiumTotal =
                premiumStrategy.Calculate(1000, 7);

            Console.WriteLine(
                $"Premium ціна: {premiumTotal}");

            IPriceStrategy standardStrategy =
                new StandardPriceStrategy();

            decimal standardTotal =
                standardStrategy.Calculate(1000, 7);

            Console.WriteLine(
                $"Standard ціна: {standardTotal}");

            break;

        case "5":

            Console.WriteLine(
                "\n=== LINQ запити ===");

            var allReservations =
                reservationRepo.GetAll().ToList();

            var activeReservations =
                allReservations
                    .Where(r =>
                        r.Status ==
                        ReservationStatus.Active)
                    .ToList();

            Console.WriteLine(
                $"\nАктивних бронювань: {activeReservations.Count}");

            var foundReservations =
                allReservations
                    .Where(r =>
                        r.GuestName.Contains("Іван"))
                    .ToList();

            Console.WriteLine(
                $"Бронювань для Іван: {foundReservations.Count}");

            var sortedReservations =
                allReservations
                    .OrderBy(r => r.Dates.Start)
                    .ToList();

            Console.WriteLine(
                "\nВідсортовані бронювання:");

            foreach (var reservation in sortedReservations)
            {
                Console.WriteLine(
                    $"{reservation.GuestName} | {reservation.Dates.Start.ToShortDateString()}");
            }

            var groupedReservations =
                allReservations
                    .GroupBy(r => r.RoomId)
                    .Select(g => new
                    {
                        RoomId = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

            Console.WriteLine(
                "\nСтатистика по кімнатах:");

            foreach (var group in groupedReservations)
            {
                Console.WriteLine(
                    $"Кімната: {group.RoomId} | Бронювань: {group.Count}");
            }

            break;

        case "0":

            running = false;

            break;

        default:

            Console.WriteLine(
                "Невірний вибір!");

            break;
    }
}

Console.WriteLine("\nПрограму завершено.");