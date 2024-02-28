namespace TodoApi.Models
{
  public class TodoItem 
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoItem(string name)
    {
      Id = GenerateUniqueId();
      Name = name;
      IsComplete = false;
    }

    // Method to generate unique Id
    private static int nextId = 1;

    private static int GenerateUniqueId()
    {
        return nextId++;
    }
  }
}