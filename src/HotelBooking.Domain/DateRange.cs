namespace HotelBooking.Domain;

public class DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateRange(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("Дата виїзду повинна бути пізніше дати заїзду.");
        
        Start = start;
        End = end;
    }

    public int TotalDays => (End - Start).Days;

    public bool Overlaps(DateRange other)
    {
        return Start < other.End && other.Start < End;
    }
}