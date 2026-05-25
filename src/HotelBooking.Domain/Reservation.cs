namespace HotelBooking.Domain;

public class Reservation
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public string GuestName { get; set; } = "";

    public DateRange Dates { get; set; } = null!;

    public ReservationStatus Status { get; set; }

    public Reservation()
    {
    }

    public Reservation(
        Guid roomId,
        string guestName,
        DateRange dates)
    {
        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException(
                "Ім'я гостя не може бути порожнім");

        Id = Guid.NewGuid();

        RoomId = roomId;

        GuestName = guestName;

        Dates = dates;

        Status = ReservationStatus.Active;
    }
}