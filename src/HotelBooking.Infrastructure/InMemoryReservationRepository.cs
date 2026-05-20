using HotelBooking.Domain;

namespace HotelBooking.Infrastructure;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly List<Reservation> _reservations = new();

    public void Add(Reservation reservation) => _reservations.Add(reservation);
    
    public IEnumerable<Reservation> GetByRoomId(Guid roomId) 
        => _reservations.Where(r => r.RoomId == roomId);
}