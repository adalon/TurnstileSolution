namespace TurnstileSolution;

/// <summary>
/// Rule 2: If the previous person exited, priority is given to people exiting (continuous flow).
/// </summary>
public class PreviousExitPreferEnterRule : IPriorityRule
{
    public Person SelectNext(IList<Person> entering, IList<Person> exiting)
    {
        // If both queues have people, exit has priority (continuous flow - previous person exited)
        if (exiting.Count > 0)
            return exiting[0]; // Lowest index due to SortedSet ordering
        
        // Only entering queue has people
        if (entering.Count > 0)
            return entering[0];
        
        throw new InvalidOperationException("No people waiting in either queue.");
    }
}
