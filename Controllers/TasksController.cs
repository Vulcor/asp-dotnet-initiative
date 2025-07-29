using Microsoft.EntityFrameworkCore;
using asp_dotnet_initiative.Models;
using asp_dotnet_initiative.Data;
using Microsoft.AspNetCore.Mvc;

namespace asp_dotnet_initiative.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TaskItems.ToListAsync();

            var viewModel = new TaskViewModel
            {
                ExistingTasks = tasks
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ExistingTasks = await _context.TaskItems.ToListAsync();
                return View("Index", model);
            }

            var newTask = new TaskItem
            {
                Title = model.Title,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            _context.TaskItems.Add(newTask);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem updatedTask)
        {
            if (!ModelState.IsValid) return View(updatedTask);

            var existingTasks = await _context.TaskItems.FindAsync(updatedTask.Id);
            if (existingTasks == null) return NotFound();

            existingTasks.Title = updatedTask.Title;
            existingTasks.IsCompleted = updatedTask.IsCompleted;
            existingTasks.UpdatedAt = DateTime.Now;

            if (updatedTask.IsCompleted && existingTasks.CompletedAt == null)
            {
                existingTasks.CompletedAt = DateTime.Now;
            }
            else
            {
                existingTasks.CompletedAt = null;
            }

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}