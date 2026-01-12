namespace TurnstileSolution;

/// <summary>
/// Manages the queues of people waiting to pass through the turnstile.
/// Automatically orders people by index (Rule 4: same direction, lower index first).
/// </summary>
public class TurnstileQueue
{
    private readonly SortedSet<Person> _entering;
    private readonly SortedSet<Person> _exiting;

    /// <summary>
    /// Gets the number of people waiting to enter.
    /// </summary>
    public int EnteringCount => _entering.Count;

    /// <summary>
    /// Gets the number of people waiting to exit.
    /// </summary>
    public int ExitingCount => _exiting.Count;

    /// <summary>
    /// Gets whether there are any people waiting in either queue.
    /// </summary>
    public bool HasWaitingPeople => _entering.Count > 0 || _exiting.Count > 0;

    /// <summary>
    /// Initializes a new instance of the TurnstileQueue class.
    /// </summary>
    public TurnstileQueue()
    {
        _entering = new SortedSet<Person>();
        _exiting = new SortedSet<Person>();
    }

    /// <summary>
    /// Adds all people who have arrived by the specified time and are not yet processed.
    /// </summary>
    /// <param name="allPeople">All people in the simulation.</param>
    /// <param name="currentTime">The current simulation time.</param>
    public void AddAvailablePeople(IEnumerable<Person> allPeople, int currentTime)
    {
        foreach (var person in allPeople)
        {
            if (!person.IsProcessed && person.ArrivalTime <= currentTime)
            {
                if (person.IsEntering)
                    _entering.Add(person);
                else
                    _exiting.Add(person);
            }
        }
    }

    /// <summary>
    /// Removes a person from the appropriate queue.
    /// </summary>
    /// <param name="person">The person to remove.</param>
    public void RemovePerson(Person person)
    {
        if (person.IsEntering)
            _entering.Remove(person);
        else
            _exiting.Remove(person);
    }

    /// <summary>
    /// Gets a list of people waiting to enter (ordered by index).
    /// </summary>
    public IList<Person> GetEnteringQueue()
    {
        return _entering.ToList();
    }

    /// <summary>
    /// Gets a list of people waiting to exit (ordered by index).
    /// </summary>
    public IList<Person> GetExitingQueue()
    {
        return _exiting.ToList();
    }

    /// <summary>
    /// Clears both queues.
    /// </summary>
    public void Clear()
    {
        _entering.Clear();
        _exiting.Clear();
    }
}
