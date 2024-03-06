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

public interface ITodoItemRepository
{
  Task<List<TodoItem>> GetAllTodoItemsAsync();
  Task<TodoItem> GetTodoItemAsync(int id);
  Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
  Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem);
  Task<bool> DeleteTodoItemAsync(int id);

}