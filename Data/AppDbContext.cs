using Microsoft.EntityFrameworkCore;
using asp_dotnet_initiative.Models;

namespace asp_dotnet_initiative.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {}

        public DbSet<TaskItem> TaskItems { get; set; } = null!;
        
    }
}