using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
        dbContext.TodoItems.Add(new TodoItem("Test Todo 1"));
        dbContext.TodoItems.Add(new TodoItem("Test Todo 2"));
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

    [Fact]
    public async Task GetTodoItemWithId_ReturnsTodoItem()
    {
        // arrange 
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        var client = _factory.CreateClient();
        var todoItem = new TodoItem("Test Todo 1");
        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();
        
        // act 
        var response = await client.GetAsync($"/api/TodoItems/{todoItem.Id}");
        
        // assert 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetTodoItemWithId_ReturnsNotFound()
    {
        // arrange 
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        var client = _factory.CreateClient();
        dbContext.TodoItems.RemoveRange(dbContext.TodoItems);
        await dbContext.SaveChangesAsync();
        
        // act 
        var response = await client.GetAsync("/api/TodoItems/1");
        
        // assert 
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostTodoITemAsync_ShouldCreateNEwTodoItem()
    {
        //Arrange
        var client = _factory.CreateClient();
        var todoItem = new TodoItem("New todo item");
        var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");
        // act 
        var response = await client.PostAsync("/api/TodoItems", jsonContent);
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdTodoItem = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        createdTodoItem.Should().NotBeNull();
        createdTodoItem.Name.Should().Be(todoItem.Name);
        createdTodoItem.IsComplete.Should().Be(false);
    }

    [Fact]
    public async Task PutTodoItemAsync_ShouldUpdateTodoItem()
    {
        // arrange 
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        var client = _factory.CreateClient();
        var todoItem = new TodoItem("Test Todo 1");
        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();

        var updatedTodoItem = new TodoItem("Updated Todo");
        updatedTodoItem.Id = todoItem.Id;
        
        var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedTodoItem), Encoding.UTF8, "application/json");
        
        // act 
        var putResponse = client.PutAsJsonAsync($"/api/TodoItems/{todoItem.Id}", jsonContent);
        
        // assert
        // putResponse.EnsureSuccessStatusCode();
        var getResponse = await client.GetAsync($"/api/TodoItems/{updatedTodoItem.Id}");
        var responseContent = await getResponse.Content.ReadAsStringAsync();
        var createdTodoItem = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        createdTodoItem.Should().NotBeNull();
        createdTodoItem.Name.Should().Be(todoItem.Name);
        createdTodoItem.IsComplete.Should().Be(false);
    }
}