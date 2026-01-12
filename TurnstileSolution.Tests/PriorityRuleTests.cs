using Xunit;
using TurnstileSolution;
using System.Collections.Generic;
using System.Linq;

namespace TurnstileSolution.Tests;

public class PriorityRuleTests
{
    [Fact]
    public void IdlePreferExitRule_BothQueuesPresent_SelectsExit()
    {
        // Arrange
        var rule = new IdlePreferExitRule();
        var entering = new List<Person> { new Person(0, 0, TurnstileDirection.Enter) };
        var exiting = new List<Person> { new Person(1, 0, TurnstileDirection.Exit) };

        // Act
        var selected = rule.SelectNext(entering, exiting);

        // Assert
        Assert.Equal(1, selected.Index);
        Assert.Equal(TurnstileDirection.Exit, selected.Direction);
    }

    [Fact]
    public void IdlePreferExitRule_OnlyEntering_SelectsEnter()
    {
        // Arrange
        var rule = new IdlePreferExitRule();
        var entering = new List<Person> { new Person(0, 0, TurnstileDirection.Enter) };
        var exiting = new List<Person>();

        // Act
        var selected = rule.SelectNext(entering, exiting);

        // Assert
        Assert.Equal(0, selected.Index);
    }

    [Fact]
    public void PreviousExitPreferEnterRule_BothQueuesPresent_SelectsExit()
    {
        // Arrange
        var rule = new PreviousExitPreferEnterRule();
        var entering = new List<Person> { new Person(0, 1, TurnstileDirection.Enter) };
        var exiting = new List<Person> { new Person(1, 1, TurnstileDirection.Exit) };

        // Act
        var selected = rule.SelectNext(entering, exiting);

        // Assert
        Assert.Equal(1, selected.Index);
        Assert.Equal(TurnstileDirection.Exit, selected.Direction);
    }

    [Fact]
    public void PreviousEnterPreferExitRule_BothQueuesPresent_SelectsEnter()
    {
        // Arrange
        var rule = new PreviousEnterPreferExitRule();
        var entering = new List<Person> { new Person(0, 1, TurnstileDirection.Enter) };
        var exiting = new List<Person> { new Person(1, 1, TurnstileDirection.Exit) };

        // Act
        var selected = rule.SelectNext(entering, exiting);

        // Assert
        Assert.Equal(0, selected.Index);
        Assert.Equal(TurnstileDirection.Enter, selected.Direction);
    }

    [Fact]
    public void SameDirectionRule_MultipleEntering_SelectsLowestIndex()
    {
        // Arrange
        var entering = new List<Person>
        {
            new Person(3, 0, TurnstileDirection.Enter),
            new Person(1, 0, TurnstileDirection.Enter),
            new Person(2, 0, TurnstileDirection.Enter)
        };

        // Act
        var sorted = entering.OrderBy(p => p.Index).ToList();

        // Assert
        Assert.Equal(1, sorted[0].Index);
        Assert.Equal(2, sorted[1].Index);
        Assert.Equal(3, sorted[2].Index);
    }
}
