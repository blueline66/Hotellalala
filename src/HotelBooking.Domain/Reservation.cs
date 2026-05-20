namespace HotelBooking.Domain;

public class Reservation
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string GuestName { get; private set; }
    public DateRange Dates { get; private set; }

    public Reservation(Guid roomId, string guestName, DateRange dates)
    {
        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException("Ім'я гостя не може бути порожнім");

        Id = Guid.NewGuid();
        RoomId = roomId;
        GuestName = guestName;
        Dates = dates;
    }
}