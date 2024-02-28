using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiDataLayerTesting;

public class TodoItemsControllerIntegrationTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TodoItemsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetTodoITems_ReturnsListOfTodoItems()
    {
        // arrange 
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        var client = _factory.CreateClient();
        dbContext.TodoItems.RemoveRange(dbContext.TodoItems);
        await dbContext.SaveChangesAsync();
        dbContext.TodoItems.Add(new TodoItem("Test Todo 1") { Name = "Test Todo 1" });
        dbContext.TodoItems.Add(new TodoItem("Test Todo 2") { Name = "Test Todo 2" });
        await dbContext.SaveChangesAsync();
        
        // act 
        var response = await client.GetAsync("/api/TodoItems");
        var responseContext = await response.Content.ReadAsStringAsync();
        var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(responseContext);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        todoItems.Should().NotBeNullOrEmpty();
        todoItems.Count.Should().Be(2);
        todoItems.Should().Contain(ti => ti.Name == "Test Todo 1" && ti.IsComplete == false);
        todoItems.Should().Contain(ti => ti.Name == "Test Todo 2" && ti.IsComplete == false);
    }
}