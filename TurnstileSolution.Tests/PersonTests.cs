using Xunit;
using TurnstileSolution.Domain;

namespace TurnstileSolution.Tests;

public class PersonTests
{
    [Fact]
    public void Person_Creation_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var person = new Person(5, 10, TurnstileDirection.Enter);

        // Assert
        Assert.Equal(5, person.Index);
        Assert.Equal(10, person.ArrivalTime);
        Assert.Equal(TurnstileDirection.Enter, person.Direction);
        Assert.False(person.IsProcessed);
        Assert.Equal(0, person.PassTime);
    }

    [Fact]
    public void Person_MarkAsProcessed_UpdatesState()
    {
        // Arrange
        var person = new Person(0, 0, TurnstileDirection.Exit);

        // Act
        person.MarkAsProcessed(15);

        // Assert
        Assert.True(person.IsProcessed);
        Assert.Equal(15, person.PassTime);
    }

    [Fact]
    public void Person_IsEntering_ReturnsCorrectValue()
    {
        // Arrange
        var entering = new Person(0, 0, TurnstileDirection.Enter);
        var exiting = new Person(1, 0, TurnstileDirection.Exit);

        // Act & Assert
        Assert.True(entering.IsEntering);
        Assert.False(exiting.IsEntering);
    }

    [Fact]
    public void Person_IsExiting_ReturnsCorrectValue()
    {
        // Arrange
        var entering = new Person(0, 0, TurnstileDirection.Enter);
        var exiting = new Person(1, 0, TurnstileDirection.Exit);

        // Act & Assert
        Assert.False(entering.IsExiting);
        Assert.True(exiting.IsExiting);
    }
}
