using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todolist.Data;
using todolist.Models;

namespace todolist.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            TaskItem = await _context.TaskItems.FindAsync(id);

            if (TaskItem == null || TaskItem.UserID != userId)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            TaskItem = await _context.TaskItems.FindAsync(id);

            if (TaskItem != null && TaskItem.UserID == userId)
            {
                _context.TaskItems.Remove(TaskItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
