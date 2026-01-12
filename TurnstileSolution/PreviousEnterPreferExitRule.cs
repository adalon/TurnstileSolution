namespace TurnstileSolution;

/// <summary>
/// Rule 3: If the previous person entered, priority is given to people entering (continuous flow).
/// </summary>
public class PreviousEnterPreferExitRule : IPriorityRule
{
    public Person SelectNext(IList<Person> entering, IList<Person> exiting)
    {
        // If both queues have people, enter has priority (continuous flow - previous person entered)
        if (entering.Count > 0)
            return entering[0]; // Lowest index due to SortedSet ordering
        
        // Only exiting queue has people
        if (exiting.Count > 0)
            return exiting[0];
        
        throw new InvalidOperationException("No people waiting in either queue.");
    }
}
