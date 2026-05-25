namespace HotelBooking.Tests;
using HotelBooking.Application;
using HotelBooking.Application.Pricing;
using HotelBooking.Infrastructure;
using HotelBooking.Domain;
using Xunit;

public class ReservationTests
{
    [Fact]
    public void Should_Create_Reservation()
    {
        var dates = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(2));

        var reservation =
            new Reservation(Guid.NewGuid(), "Іван", dates);

        Assert.Equal("Іван", reservation.GuestName);
    }

    [Fact]
    public void Should_Throw_When_GuestName_Empty()
    {
        var dates = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(2));

        Assert.Throws<ArgumentException>(() =>
            new Reservation(Guid.NewGuid(), "", dates));
    }

    [Fact]
    public void Should_Have_Active_Status_By_Default()
    {
        var dates = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(2));

        var reservation =
            new Reservation(Guid.NewGuid(), "Іван", dates);

        Assert.Equal(
            ReservationStatus.Active,
            reservation.Status);
    }

    [Fact]
    public void Should_Change_Status()
    {
        var dates = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(2));

        var reservation =
            new Reservation(Guid.NewGuid(), "Іван", dates);

        reservation.Status = ReservationStatus.Cancelled;

        Assert.Equal(
            ReservationStatus.Cancelled,
            reservation.Status);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void Should_Create_DateRange_With_Valid_Days(int days)
    {
        var range = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(days));

        Assert.NotNull(range);
    }

    [Fact]
    public void Should_Throw_When_EndDate_Before_Start()
    {
        Assert.Throws<ArgumentException>(() =>
            new DateRange(
                DateTime.Now,
                DateTime.Now.AddDays(-1)));
    }

    [Fact]
    public void Should_Detect_Overlap()
    {
        var first = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(5));

        var second = new DateRange(
            DateTime.Now.AddDays(2),
            DateTime.Now.AddDays(6));

        Assert.True(first.Overlaps(second));
    }

    [Fact]
    public void Should_Not_Detect_Overlap()
    {
        var first = new DateRange(
            DateTime.Now,
            DateTime.Now.AddDays(2));

        var second = new DateRange(
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(7));

        Assert.False(first.Overlaps(second));
    }

    [Fact]
    public void Standard_Strategy_Should_Calculate_Total()
    {
        IPriceStrategy strategy =
            new StandardPriceStrategy();

        var result = strategy.Calculate(1000, 5);

        Assert.Equal(5000, result);
    }

    [Fact]
    public void Premium_Strategy_Should_Apply_Discount()
    {
        IPriceStrategy strategy =
            new PremiumDiscountStrategy();

        var result = strategy.Calculate(1000, 7);

        Assert.Equal(5600, result);
    }

    [Fact]
    public void Premium_Strategy_Should_Not_Apply_Discount()
    {
        IPriceStrategy strategy =
            new PremiumDiscountStrategy();

        var result = strategy.Calculate(1000, 2);

        Assert.Equal(2000, result);
    }

    [Fact]
    public void Should_Create_StandardRoom()
    {
        var room = new StandardRoom("101", 1000);

        Assert.Equal("101", room.Number);
    }

    [Fact]
    public void Should_Create_PremiumRoom()
    {
        var room = new PremiumRoom("201", 2000, 500);

        Assert.Equal("201", room.Number);
    }

    [Fact]
    public void PremiumRoom_Should_Have_ExtraCharge()
    {
        var room = new PremiumRoom("201", 2000, 500);

        Assert.Equal(500, room.ExtraCharge);
    }

    [Fact]
    public void Repository_Should_Return_Rooms()
    {
        var repo = new InMemoryRoomRepository();

        var rooms = repo.GetAll();

        Assert.NotEmpty(rooms);
    }

    [Fact]
    public void Repository_Should_Find_Room_By_Id()
    {
        var repo = new InMemoryRoomRepository();

        var room = repo.GetAll().First();

        var found = repo.GetById(room.Id);

        Assert.NotNull(found);
    }

    [Fact]
    public void ReservationRepository_Should_Add_Reservation()
    {
        var repo =
            new InMemoryReservationRepository();

        var reservation =
            new Reservation(
                Guid.NewGuid(),
                "Іван",
                new DateRange(
                    DateTime.Now,
                    DateTime.Now.AddDays(2)));

        repo.Add(reservation);

        Assert.Single(
            repo.GetByRoomId(reservation.RoomId));
    }

    [Fact]
    public void Empty_Reservation_List_Should_Return_Empty()
    {
        var repo =
            new InMemoryReservationRepository();

        var result =
            repo.GetByRoomId(Guid.NewGuid());

        Assert.Empty(result);
    }

    [Fact]
    public void Booking_Should_Succeed()
    {
        var roomRepo =
            new InMemoryRoomRepository();

        var reservationRepo =
            new InMemoryReservationRepository();

        var service =
            new BookingService(
                roomRepo,
                reservationRepo);

        var room = roomRepo.GetAll().First();

        var result = service.TryBookRoom(
            room.Id,
            "Іван",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            out _);

        Assert.True(result);
    }

    [Fact]
    public void Double_Booking_Should_Fail()
    {
        var roomRepo =
            new InMemoryRoomRepository();

        var reservationRepo =
            new InMemoryReservationRepository();

        var service =
            new BookingService(
                roomRepo,
                reservationRepo);

        var room = roomRepo.GetAll().First();

        service.TryBookRoom(
            room.Id,
            "Іван",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            out _);

        var result = service.TryBookRoom(
            room.Id,
            "Петро",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            out _);

        Assert.False(result);
    }
}