namespace HotelBooking.Domain;
public interface IRoomRepository
{
    Room GetById(Guid id);
    IEnumerable<Room> GetAll();
}