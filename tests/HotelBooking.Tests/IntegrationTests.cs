using HotelBooking.Application;
using HotelBooking.Domain;
using HotelBooking.Infrastructure;
using Xunit;

namespace HotelBooking.Tests;

public class IntegrationTests
{
    [Fact]
    public void Should_Save_Reservation_To_Json()
    {
        if (File.Exists("test.json"))
        {
            File.Delete("test.json");
        }
        // Arrange
        var roomRepo = new InMemoryRoomRepository();

        var reservationRepo =
            new JsonReservationRepository("test.json");

        var service =
            new BookingService(roomRepo, reservationRepo);

        var room = roomRepo.GetAll().First();

        // Act
        service.TryBookRoom(
            room.Id,
            "Іван",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            out _);

        var reservations =
            reservationRepo.GetAll();

        // Assert
        Assert.Single(reservations);
    }
    [Fact]
    public void Should_Prevent_Double_Booking()
    {
        var roomRepo = new InMemoryRoomRepository();

        var reservationRepo =
            new JsonReservationRepository("double.json");

        var service =
            new BookingService(roomRepo, reservationRepo);

        var room = roomRepo.GetAll().First();

        service.TryBookRoom(
            room.Id,
            "Іван",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            out _);

        var result = service.TryBookRoom(
            room.Id,
            "Петро",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            out _);

        Assert.False(result);
    }

    [Fact]
    public void Should_Return_Message_When_Booking_Fails()
    {
        var roomRepo = new InMemoryRoomRepository();

        var reservationRepo =
            new JsonReservationRepository("message.json");

        var service =
            new BookingService(roomRepo, reservationRepo);

        var room = roomRepo.GetAll().First();

        service.TryBookRoom(
            room.Id,
            "Іван",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            out _);

        service.TryBookRoom(
            room.Id,
            "Петро",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            out string message);

        Assert.Contains("Помилка", message);
    }

    [Fact]
    public void Should_Load_Data_From_Json()
    {
        var repo =
            new JsonReservationRepository("load.json");

        var reservation =
            new Reservation(
                Guid.NewGuid(),
                "Іван",
                new DateRange(
                    DateTime.Now,
                    DateTime.Now.AddDays(2)));

        repo.Add(reservation);

        var loaded =
            repo.GetAll();

        Assert.NotEmpty(loaded);
    }

    [Fact]
    public void Should_Filter_Reservations_With_Linq()
    {
        var repo =
            new JsonReservationRepository("linq.json");

        repo.Add(
            new Reservation(
                Guid.NewGuid(),
                "Іван",
                new DateRange(
                    DateTime.Now,
                    DateTime.Now.AddDays(2))));

        var result = repo
            .GetAll()
            .Where(r => r.GuestName.Contains("Іван"));

        Assert.Single(result);
    }

    [Fact]
    public void Should_Group_Reservations()
    {
        var repo =
            new JsonReservationRepository("group.json");

        var roomId = Guid.NewGuid();

        repo.Add(
            new Reservation(
                roomId,
                "Іван",
                new DateRange(
                    DateTime.Now,
                    DateTime.Now.AddDays(2))));

        repo.Add(
            new Reservation(
                roomId,
                "Петро",
                new DateRange(
                    DateTime.Now.AddDays(3),
                    DateTime.Now.AddDays(5))));

        var grouped = repo
            .GetAll()
            .GroupBy(r => r.RoomId);

        Assert.Single(grouped);
    }

    [Fact]
    public void Should_Sort_Reservations()
    {
        var repo =
            new JsonReservationRepository("sort.json");

        repo.Add(
            new Reservation(
                Guid.NewGuid(),
                "Іван",
                new DateRange(
                    DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(7))));

        repo.Add(
            new Reservation(
                Guid.NewGuid(),
                "Петро",
                new DateRange(
                    DateTime.Now,
                    DateTime.Now.AddDays(2))));

        var sorted = repo
            .GetAll()
            .OrderBy(r => r.Dates.Start)
            .ToList();

        Assert.Equal("Петро", sorted.First().GuestName);
    }

    [Fact]
    public void Should_Handle_Invalid_Data()
    {
        var roomRepo = new InMemoryRoomRepository();

        var reservationRepo =
            new JsonReservationRepository("invalid.json");

        var service =
            new BookingService(roomRepo, reservationRepo);

        var room = roomRepo.GetAll().First();

        var result = service.TryBookRoom(
            room.Id,
            "",
            DateTime.Now,
            DateTime.Now.AddDays(2),
            out _);

        Assert.False(result);
    }
}