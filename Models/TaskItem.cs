using System.ComponentModel.DataAnnotations;
using asp_dotnet_initiative.Models;

namespace asp_dotnet_initiative.Models
{
  public class TaskItem
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static implicit operator List<object>(TaskItem v)
    {
      throw new NotImplementedException();
    }
  }
}