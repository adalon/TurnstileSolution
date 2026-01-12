namespace TurnstileSolution;

/// <summary>
/// Defines a strategy for selecting the next person to pass through the turnstile
/// when multiple people are waiting.
/// </summary>
public interface IPriorityRule
{
    /// <summary>
    /// Selects the next person to pass through the turnstile from the waiting queues.
    /// </summary>
    /// <param name="entering">List of people waiting to enter, ordered by index.</param>
    /// <param name="exiting">List of people waiting to exit, ordered by index.</param>
    /// <returns>The person who should pass through next.</returns>
    Person SelectNext(IList<Person> entering, IList<Person> exiting);
}
