namespace TurnstileSolution;

/// <summary>
/// Represents a person waiting to pass through the turnstile.
/// </summary>
public class Person : IComparable<Person>
{
    /// <summary>
    /// Gets the person's index (identifier) in the input array.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Gets the time when the person arrives at the turnstile.
    /// </summary>
    public int ArrivalTime { get; init; }

    /// <summary>
    /// Gets the direction the person wants to go (Enter or Exit).
    /// </summary>
    public TurnstileDirection Direction { get; init; }

    /// <summary>
    /// Gets whether this person has already passed through the turnstile.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Gets the time when the person passed through the turnstile.
    /// </summary>
    public int PassTime { get; private set; }

    /// <summary>
    /// Gets whether this person is entering the building.
    /// </summary>
    public bool IsEntering => Direction == TurnstileDirection.Enter;

    /// <summary>
    /// Gets whether this person is exiting the building.
    /// </summary>
    public bool IsExiting => Direction == TurnstileDirection.Exit;

    /// <summary>
    /// Initializes a new instance of the Person class.
    /// </summary>
    /// <param name="index">The person's index in the input array.</param>
    /// <param name="arrivalTime">The time when the person arrives.</param>
    /// <param name="direction">The direction the person wants to go.</param>
    public Person(int index, int arrivalTime, TurnstileDirection direction)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
        if (arrivalTime < 0)
            throw new ArgumentOutOfRangeException(nameof(arrivalTime), "Arrival time cannot be negative.");

        Index = index;
        ArrivalTime = arrivalTime;
        Direction = direction;
        IsProcessed = false;
        PassTime = 0;
    }

    /// <summary>
    /// Marks this person as having passed through the turnstile at the specified time.
    /// </summary>
    /// <param name="time">The time when the person passed through.</param>
    public void MarkAsProcessed(int time)
    {
        if (time < ArrivalTime)
            throw new ArgumentException("Pass time cannot be before arrival time.", nameof(time));

        IsProcessed = true;
        PassTime = time;
    }

    /// <summary>
    /// Compares this person to another based on index (for SortedSet ordering - Rule 4).
    /// </summary>
    /// <param name="other">The other person to compare to.</param>
    /// <returns>Negative if this person's index is lower, positive if higher, zero if equal.</returns>
    public int CompareTo(Person? other)
    {
        if (other == null)
            return 1;

        return Index.CompareTo(other.Index);
    }
}
