using HotelBooking.Domain;

namespace HotelBooking.Infrastructure;

public class InMemoryRoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = new();

    public InMemoryRoomRepository()
    {
        // Додаємо кілька кімнат для тесту
        _rooms.Add(new StandardRoom("101", 1000m));
        _rooms.Add(new PremiumRoom("201", 2500m, 500m));
    }

    public IEnumerable<Room> GetAll() => _rooms;
    public Room GetById(Guid id) => _rooms.FirstOrDefault(r => r.Id == id);
}