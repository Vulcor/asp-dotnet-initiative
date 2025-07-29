using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace asp_dotnet_initiative.Models
{
  public class TaskViewModel
  {
    [Required(ErrorMessage = "Title cannot be empty.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    public List<TaskItem> ExistingTasks { get; set; } = new();
  }
}