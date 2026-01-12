using Xunit;
using TurnstileSolution.Domain;
using TurnstileSolution.Simulation;

namespace TurnstileSolution.Tests;

public class TurnstileSimulatorTests
{
    /// <summary>
    /// Example 1: Tests Rule 1 (idle prefers exit) and basic simulation
    /// Input: n = 4, times = [0, 0, 1, 5], direction = [0, 1, 1, 0]
    /// Expected: [2, 0, 1, 5]
    /// 
    /// Explanation:
    /// - Time 0: Person 0 (enter) and Person 1 (exit) arrive
    ///   ? Rule 1: Turnstile idle, exit has priority ? Person 1 exits at time 0
    /// - Time 1: Person 0 (enter) and Person 2 (exit) waiting
    ///   ? Previous was exit, so Rule 2: enter goes first ? Person 2 exits at time 1
    /// - Time 2: Person 0 (enter) still waiting
    ///   ? Previous was exit, so enter goes ? Person 0 enters at time 2
    /// - Time 5: Person 3 (enter) arrives ? enters at time 5
    /// </summary>
    [Fact]
    public void Example1_IdlePreferExit_And_RuleSwitching()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 0, TurnstileDirection.Exit),
            new Person(2, 1, TurnstileDirection.Exit),
            new Person(3, 5, TurnstileDirection.Enter)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(2, results[0]); // Person 0 passes at time 2
        Assert.Equal(0, results[1]); // Person 1 passes at time 0
        Assert.Equal(1, results[2]); // Person 2 passes at time 1
        Assert.Equal(5, results[3]); // Person 3 passes at time 5
    }

    /// <summary>
    /// Example 2: Tests Rule 2 and Rule 3 (alternating priority)
    /// Input: n = 5, times = [0, 1, 1, 3, 3], direction = [0, 0, 1, 0, 1]
    /// Expected: [0, 2, 1, 4, 3]
    /// 
    /// Explanation:
    /// - Time 0: Person 0 (enter) arrives alone ? enters at time 0
    /// - Time 1: Person 1 (enter) and Person 2 (exit) arrive
    ///   ? Previous was enter, so Rule 3: exit goes first ? Person 2 exits at time 1
    /// - Time 2: Person 1 (enter) waiting
    ///   ? Previous was exit, so Rule 2: enter goes ? Person 1 enters at time 2
    /// - Time 3: Person 3 (enter) and Person 4 (exit) arrive
    ///   ? Previous was enter, so Rule 3: exit goes ? Person 4 exits at time 3
    /// - Time 4: Person 3 (enter) waiting
    ///   ? Previous was exit, so enter goes ? Person 3 enters at time 4
    /// </summary>
    [Fact]
    public void Example2_AlternatingPriority()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 1, TurnstileDirection.Enter),
            new Person(2, 1, TurnstileDirection.Exit),
            new Person(3, 3, TurnstileDirection.Enter),
            new Person(4, 3, TurnstileDirection.Exit)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(0, results[0]); // Person 0 passes at time 0
        Assert.Equal(2, results[1]); // Person 1 passes at time 2
        Assert.Equal(1, results[2]); // Person 2 passes at time 1
        Assert.Equal(4, results[3]); // Person 3 passes at time 4
        Assert.Equal(3, results[4]); // Person 4 passes at time 3
    }

    /// <summary>
    /// Test Rule 4: Same direction, lower index goes first
    /// </summary>
    [Fact]
    public void SameDirection_LowerIndexFirst()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 0, TurnstileDirection.Enter),
            new Person(2, 0, TurnstileDirection.Enter)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(0, results[0]); // Person 0 passes at time 0
        Assert.Equal(1, results[1]); // Person 1 passes at time 1
        Assert.Equal(2, results[2]); // Person 2 passes at time 2
    }

    /// <summary>
    /// Test single person scenario
    /// </summary>
    [Fact]
    public void SinglePerson_PassesImmediately()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 5, TurnstileDirection.Exit)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(5, results[0]); // Person passes at their arrival time
    }

    /// <summary>
    /// Test time gaps - turnstile becomes idle
    /// </summary>
    [Fact]
    public void TimeGaps_TurnstileBecomesIdle()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 5, TurnstileDirection.Enter),
            new Person(2, 5, TurnstileDirection.Exit)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(0, results[0]); // Person 0 passes at time 0
        Assert.Equal(6, results[1]); // Person 1 passes at time 6 (exit goes first when idle)
        Assert.Equal(5, results[2]); // Person 2 passes at time 5 (exit priority)
    }

    /// <summary>
    /// Test large time values (constraint: up to 10^9)
    /// </summary>
    [Fact]
    public void LargeTimeValues_HandledCorrectly()
    {
        // Arrange
        var simulator = new TurnstileSimulator();
        var people = new[]
        {
            new Person(0, 1000000000, TurnstileDirection.Enter),
            new Person(1, 1000000000, TurnstileDirection.Exit)
        };

        // Act
        var results = simulator.Simulate(people);

        // Assert
        Assert.Equal(1000000001, results[0]); // Enter goes second (exit has idle priority)
        Assert.Equal(1000000000, results[1]); // Exit goes first
    }
}
