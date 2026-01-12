namespace TurnstileSolution;

/// <summary>
/// Simulates people passing through a turnstile according to the priority rules.
/// </summary>
public class TurnstileSimulator
{
    private readonly IdlePreferExitRule _idleRule;
    private readonly PreviousExitPreferEnterRule _previousExitRule;
    private readonly PreviousEnterPreferExitRule _previousEnterRule;

    /// <summary>
    /// Initializes a new instance of the TurnstileSimulator class.
    /// </summary>
    public TurnstileSimulator()
    {
        _idleRule = new IdlePreferExitRule();
        _previousExitRule = new PreviousExitPreferEnterRule();
        _previousEnterRule = new PreviousEnterPreferExitRule();
    }

    /// <summary>
    /// Simulates the turnstile passage for all people.
    /// </summary>
    /// <param name="people">Array of people waiting to pass through.</param>
    /// <returns>Array of pass times indexed by person index.</returns>
    public int[] Simulate(Person[] people)
    {
        if (people == null || people.Length == 0)
            throw new ArgumentException("People array cannot be null or empty.", nameof(people));

        ValidateInput(people);

        int n = people.Length;
        int[] results = new int[n];
        int currentTime = 0;
        TurnstileDirection? lastDirection = null; // null = idle (not used in previous second)
        int processedCount = 0;

        while (processedCount < n)
        {
            // Find all people who have arrived by current time and are not yet processed
            var available = new List<Person>();
            foreach (var person in people)
            {
                if (!person.IsProcessed && person.ArrivalTime <= currentTime)
                {
                    available.Add(person);
                }
            }

            if (available.Count == 0)
            {
                // No one is waiting - advance time to next arrival
                int nextArrival = GetNextArrivalTime(people);
                currentTime = nextArrival;
                lastDirection = null; // Turnstile becomes idle
                continue;
            }

            // Separate into entering and exiting, sorted by index
            var entering = available.Where(p => p.IsEntering).OrderBy(p => p.Index).ToList();
            var exiting = available.Where(p => p.IsExiting).OrderBy(p => p.Index).ToList();

            // Select the next person based on priority rules
            Person nextPerson = SelectNextPerson(entering, exiting, lastDirection);

            // Process the person
            nextPerson.MarkAsProcessed(currentTime);
            results[nextPerson.Index] = currentTime;
            lastDirection = nextPerson.Direction;
            processedCount++;

            // Advance time
            currentTime++;
        }

        return results;
    }

    /// <summary>
    /// Selects the next person to pass through based on the current state.
    /// </summary>
    private Person SelectNextPerson(IList<Person> entering, IList<Person> exiting, TurnstileDirection? lastDirection)
    {
        IPriorityRule rule;

        if (lastDirection == null)
        {
            // Rule 1: Turnstile was idle, exit has priority
            rule = _idleRule;
        }
        else if (lastDirection == TurnstileDirection.Exit)
        {
            // Rule 2: Previous person exited, enter has priority
            rule = _previousExitRule;
        }
        else // lastDirection == TurnstileDirection.Enter
        {
            // Rule 3: Previous person entered, exit has priority
            rule = _previousEnterRule;
        }

        return rule.SelectNext(entering, exiting);
    }

    /// <summary>
    /// Gets the next arrival time from unprocessed people.
    /// </summary>
    private int GetNextArrivalTime(Person[] people)
    {
        int nextArrival = int.MaxValue;
        foreach (var person in people)
        {
            if (!person.IsProcessed)
            {
                nextArrival = Math.Min(nextArrival, person.ArrivalTime);
            }
        }
        return nextArrival;
    }

    /// <summary>
    /// Validates the input array of people.
    /// </summary>
    private void ValidateInput(Person[] people)
    {
        // Check for duplicate indices
        var indices = new HashSet<int>();
        foreach (var person in people)
        {
            if (!indices.Add(person.Index))
                throw new ArgumentException($"Duplicate person index found: {person.Index}", nameof(people));
        }

        // Verify array is sorted by arrival time
        for (int i = 1; i < people.Length; i++)
        {
            if (people[i].ArrivalTime < people[i - 1].ArrivalTime)
                throw new ArgumentException("People array must be sorted by arrival time.", nameof(people));
        }

        // Verify indices match array positions
        for (int i = 0; i < people.Length; i++)
        {
            if (people[i].Index != i)
                throw new ArgumentException($"Person at position {i} has incorrect index {people[i].Index}. Expected index {i}.", nameof(people));
        }
    }
}
