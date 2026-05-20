namespace HotelBooking.Domain;
public interface IReservationRepository
{
    void Add(Reservation reservation);
    IEnumerable<Reservation> GetByRoomId(Guid roomId);
}