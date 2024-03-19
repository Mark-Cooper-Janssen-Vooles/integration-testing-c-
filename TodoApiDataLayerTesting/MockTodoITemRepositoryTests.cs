using System.Data.Common;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiDataLayerTesting;

public class MockTodoITemRepositoryTests
{
    private DbContextOptions<TodoContext> _options;

    public MockTodoITemRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlite(CreateInMemoryDatabase()).Options;
        using (var context = new TodoContext(_options))
        {
            context.Database.EnsureCreated();
        }
    }

    private static DbConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        return connection;
    }

    private static DbContextOptions<TodoContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<TodoContext>().UseSqlite(CreateInMemoryDatabase()).Options;
    }

    [Fact]
    public async Task GetTodoItemByIdAsync_ReturnsTodoItem()
    {
        // arrange
        DbContextOptions<TodoContext> options = CreateOptions();
        using (var context = new TodoContext(options))
        {
            var repository = new MockTodoItemRepository(options);
            var todoItem = new TodoItem("Test item");
            await repository.CreateTodoItemAsync(todoItem);
            // act 
            var result = await repository.GetTodoItemAsync(todoItem.Id);
            // assert
            Assert.Equal(todoItem.Id, result.Id);
            todoItem.Id.Should().Be(result.Id);
        }
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateItem()
    {
        // arrange
        DbContextOptions<TodoContext> options = CreateOptions();
        using (var context = new TodoContext(options))
        {
            var repository = new MockTodoItemRepository(options);
            var todoItem = new TodoItem("Test item");
            // act 
            var result = await repository.CreateTodoItemAsync(todoItem);
            // assert
            result.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateItem()
    {
        // arrange
        DbContextOptions<TodoContext> options = CreateOptions();
        using (var context = new TodoContext(options))
        {
            var repository = new MockTodoItemRepository(options);
            var todoItem = new TodoItem("Test item");
            await repository.CreateTodoItemAsync(todoItem);
            // act 
            todoItem.Name = "Test item updated";
            await repository.UpdateTodoItemAsync(todoItem);
            var result = await repository.GetTodoItemAsync(todoItem.Id);
            // assert
            result.Name.Should().Be(todoItem.Name);
        }
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteItem()
    {
        // arrange
        DbContextOptions<TodoContext> options = CreateOptions();
        using (var context = new TodoContext(options))
        {
            var repository = new MockTodoItemRepository(options);
            var todoItem = new TodoItem("Test item");
            await repository.CreateTodoItemAsync(todoItem);
            // act 
            var deleteResult = await repository.DeleteTodoItemAsync(todoItem.Id);
            var result = await repository.GetTodoItemAsync(todoItem.Id);
            // assert
            deleteResult.Should().Be(true);
            result.Should().Be(null);
        }
    }
}