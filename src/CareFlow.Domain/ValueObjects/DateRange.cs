using CareFlow.Domain.Exceptions;

namespace CareFlow.Domain.ValueObjects;

public record DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateRange(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            throw new DomainException("End date must be after start date.");
        }

        Start = start;
        End = end;
    }

    public bool Overlaps(DateRange other)
    {
        return Start < other.End && End > other.Start;
    }

    public TimeSpan Duration => End - Start;
}
