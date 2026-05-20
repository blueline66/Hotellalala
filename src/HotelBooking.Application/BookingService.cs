using HotelBooking.Domain;

namespace HotelBooking.Application;

public class BookingService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IReservationRepository _reservationRepository;

    public BookingService(IRoomRepository roomRepo, IReservationRepository resRepo)
    {
        _roomRepository = roomRepo;
        _reservationRepository = resRepo;
    }

    public bool TryBookRoom(Guid roomId, string guestName, DateTime start, DateTime end, out string message)
    {
        try
        {
            var dates = new DateRange(start, end);
            
            // Перевірка на перетин дат
            var existingReservations = _reservationRepository.GetByRoomId(roomId);
            foreach (var res in existingReservations)
            {
                if (res.Dates.Overlaps(dates))
                {
                    message = "Помилка: Номер вже зайнятий на ці дати!";
                    return false;
                }
            }

            var reservation = new Reservation(roomId, guestName, dates);
            _reservationRepository.Add(reservation);
            
            message = $"Успіх! Бронювання створено. ID: {reservation.Id}";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Помилка: {ex.Message}";
            return false;
        }
    }
}