using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todolist.Data;
using todolist.Models;

namespace todolist.Pages.Tasks
{
    public class DoneModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DoneModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null || taskItem.UserID != userId)
            {
                return NotFound();
            }

            taskItem.IsCompleted = true;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
