using Xunit;
using TurnstileSolution.Domain;
using TurnstileSolution.Simulation;
using System.Linq;

namespace TurnstileSolution.Tests;

public class TurnstileQueueTests
{
    [Fact]
    public void AddPeople_SeparatesIntoEnteringAndExiting()
    {
        // Arrange
        var queue = new TurnstileQueue();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 0, TurnstileDirection.Exit),
            new Person(2, 0, TurnstileDirection.Enter),
            new Person(3, 0, TurnstileDirection.Exit)
        };

        // Act
        queue.AddAvailablePeople(people, 0);

        // Assert
        Assert.Equal(2, queue.EnteringCount);
        Assert.Equal(2, queue.ExitingCount);
    }

    [Fact]
    public void AddPeople_OnlyAddsThoseWhoHaveArrived()
    {
        // Arrange
        var queue = new TurnstileQueue();
        var people = new[]
        {
            new Person(0, 0, TurnstileDirection.Enter),
            new Person(1, 5, TurnstileDirection.Exit),
            new Person(2, 10, TurnstileDirection.Enter)
        };

        // Act
        queue.AddAvailablePeople(people, 5);

        // Assert
        Assert.Equal(1, queue.EnteringCount); // Person 0
        Assert.Equal(1, queue.ExitingCount);  // Person 1
    }

    [Fact]
    public void HasWaitingPeople_ReturnsTrueWhenPeoplePresent()
    {
        // Arrange
        var queue = new TurnstileQueue();
        var people = new[] { new Person(0, 0, TurnstileDirection.Enter) };

        // Act
        queue.AddAvailablePeople(people, 0);

        // Assert
        Assert.True(queue.HasWaitingPeople);
    }

    [Fact]
    public void HasWaitingPeople_ReturnsFalseWhenEmpty()
    {
        // Arrange
        var queue = new TurnstileQueue();

        // Assert
        Assert.False(queue.HasWaitingPeople);
    }

    [Fact]
    public void RemovePerson_RemovesFromCorrectQueue()
    {
        // Arrange
        var queue = new TurnstileQueue();
        var person = new Person(0, 0, TurnstileDirection.Enter);
        var people = new[] { person };
        queue.AddAvailablePeople(people, 0);

        // Act
        queue.RemovePerson(person);

        // Assert
        Assert.Equal(0, queue.EnteringCount);
        Assert.False(queue.HasWaitingPeople);
    }
}
