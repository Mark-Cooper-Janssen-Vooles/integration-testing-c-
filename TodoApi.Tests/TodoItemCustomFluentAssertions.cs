using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using FluentAssertions;
using TodoApi.Models;

namespace TodoApi.Tests;

public class TodoItemAssertions : ReferenceTypeAssertions<TodoItem, TodoItemAssertions>
{
    public TodoItemAssertions(TodoItem instance): base(instance) {}
    protected override string Identifier => "TodoItem";
}

public static class TodoItemAssertionsExtensions
{
    public static TodoItemAssertions Should(this TodoItem todoItem)
    {
        return new TodoItemAssertions(todoItem);
    }

    public static AndConstraint<TodoItemAssertions> HaveTheSameNameAndIsCompleteValues(
        this TodoItemAssertions todo1, TodoItem todo2)
    {
        var todo1Name = todo1.Subject.Name;
        var todo2Name = todo2.Name;
        var todo1IsComplete = todo1.Subject.IsComplete;
        var todo2IsComplete = todo2.IsComplete;
        
        Execute.Assertion.ForCondition(todo1Name == todo2Name).FailWith("names are not the same!");
        Execute.Assertion.ForCondition(todo1IsComplete == todo2IsComplete).FailWith("isComplete is not the same!");

        return new AndConstraint<TodoItemAssertions>(todo1);
    }
}