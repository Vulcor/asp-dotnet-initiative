using System.Text.Json;
using asp_dotnet_initiative.Data;
using asp_dotnet_initiative.Models;

public static class TaskItemSeeder
{
  public static void Seed(AppDbContext context, IWebHostEnvironment env)
  {

    if (context.TaskItems.Any())
    {
      return;
    }

    var filePath = Path.Combine(env.ContentRootPath, "Data", "Seeders", "seed-data", "tasks.json");

    if (!File.Exists(filePath))
    {
      throw new FileNotFoundException($"Seed file not found: {filePath}");
    }

    var json = File.ReadAllText(filePath);
    var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    });

    if (tasks != null)
    {
      context.TaskItems.AddRange(tasks);
      context.SaveChanges();
    }
  }
}