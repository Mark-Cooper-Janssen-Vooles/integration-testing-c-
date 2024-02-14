using FluentAssertions;
using TodoApi.Models;
namespace TodoApi.Tests;

public class TodoItemTests
{
    [Fact]
    public void Should_Have_Correct_Name()
    {
        // arrange
        var todo = new TodoItem("Test name");
        // act 
        
        // assert
        todo.Name.Should().Be("Test name");
    }

    [Fact]
    public void IsComplete_Should_Default_ToFalse()
    {
        var todo = new TodoItem("Test");
        todo.IsComplete.Should().Be(false);
    }
    
    [Fact]
    public void IsComplete_Should_Update_Correctly()
    {
        var todo = new TodoItem("Test");
        todo.IsComplete = true;
        todo.IsComplete.Should().Be(true);
    }

    [Fact]
    public void Should_Have_Same_IsComplete_And_Name()
    {
        var todo1 = new TodoItem("Test");
        var todo2 = new TodoItem("Test");
        todo1.Should().HaveTheSameNameAndIsCompleteValues(todo2);
    }

    [Fact]
    public void Should_Have_Incremented_Id()
    {
        var todo1 = new TodoItem("Test");
        var todo2 = new TodoItem("Test");
        var todo3 = new TodoItem("Test");
        var todo4 = new TodoItem("Test");

        new[] { todo1.Id, todo2.Id, todo3.Id, todo4.Id }
            .Should().OnlyHaveUniqueItems();
    }
}