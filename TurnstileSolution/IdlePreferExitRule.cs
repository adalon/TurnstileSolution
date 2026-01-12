namespace TurnstileSolution;

/// <summary>
/// Rule 1: When the turnstile was not used in the previous second (idle),
/// priority is given to people exiting.
/// </summary>
public class IdlePreferExitRule : IPriorityRule
{
    public Person SelectNext(IList<Person> entering, IList<Person> exiting)
    {
        // If both queues have people, exit has priority when idle
        if (exiting.Count > 0)
            return exiting[0]; // Lowest index due to SortedSet ordering
        
        // Only entering queue has people
        if (entering.Count > 0)
            return entering[0];
        
        throw new InvalidOperationException("No people waiting in either queue.");
    }
}
