using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiDataLayerTesting;

public class MockTodoItemRepository: ITodoItemRepository
{
    private List<TodoItem> _todoItems;

    public MockTodoItemRepository(DbContextOptions<TodoContext> options)
    {
        _todoItems = new List<TodoItem>();
        using (var context = new TodoContext(options))
        {
            // ensure db is created
            context.Database.EnsureCreated();
        }
    }
    
    public Task<List<TodoItem>> GetAllTodoItemsAsync()
    {
        return Task.FromResult(_todoItems);
    }

    public Task<TodoItem> GetTodoItemAsync(int id)
    {
        var todoItem = _todoItems.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(todoItem);
    }

    public Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
    {
        _todoItems.Add(todoItem);
        return Task.FromResult(todoItem);
    }

    public Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem)
    {
        var existingItem = _todoItems.FirstOrDefault(x => x.Id == todoItem.Id);
        if (existingItem != null)
        {
            _todoItems.Remove(existingItem);
            _todoItems.Add(todoItem);
        }

        return Task.FromResult(todoItem);
    }

    public Task<bool> DeleteTodoItemAsync(int id)
    {
        var todoItem = _todoItems.FirstOrDefault(x => x.Id == id);
        if (todoItem != null)
        {
            _todoItems.Remove(todoItem);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public void Reset()
    {
        _todoItems.Clear();
    }
}