using System.Text.Json;
using HotelBooking.Domain;

namespace HotelBooking.Infrastructure;

public class JsonReservationRepository : IReservationRepository
{
    private readonly string _filePath;

    private readonly List<Reservation> _reservations;

    public JsonReservationRepository(string filePath)
    {
        _filePath = filePath;

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);

            _reservations =
                JsonSerializer.Deserialize<List<Reservation>>(json)
                ?? new List<Reservation>();
        }
        else
        {
            _reservations = new List<Reservation>();
        }
    }

    public void Add(Reservation reservation)
    {
        _reservations.Add(reservation);

        Save();
    }

    public IEnumerable<Reservation> GetByRoomId(Guid roomId)
    {
        return _reservations
            .Where(r => r.RoomId == roomId);
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _reservations;
    }

    private void Save()
    {
        var json =
            JsonSerializer.Serialize(
                _reservations,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(_filePath, json);
    }
}