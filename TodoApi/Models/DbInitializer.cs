using TodoApi.Data;

namespace TodoApi.Models
{
  public static class DbInitializer
  {
    public static void Initialize(TodoContext context) 
    {
      context.Database.EnsureCreated();
      // look to see if DB is seeded already
      if (context.TodoItems.Any())
      {
        return;
      }
      // seed db with test data
      var todos = new TodoItem[]
      {
        new TodoItem { Name = "Learn C#"},
        new TodoItem { Name = "Learn Integration testing"},
        new TodoItem { Name = "Eat lunch"},
      };

      foreach (var todo in todos)
      {
        context.TodoItems.Add(todo);
      }
      context.SaveChanges();
    }
  }
}