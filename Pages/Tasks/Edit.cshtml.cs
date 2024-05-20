using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using todolist.Data;
using todolist.Models;

namespace todolist.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
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

        public async Task<IActionResult> OnPostAsync()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var existingTaskItem = await _context.TaskItems.FindAsync(TaskItem.TaskID);
            if (existingTaskItem == null)
            {
                return NotFound();
            }

            if (existingTaskItem.UserID != userId)
            {
                return NotFound();
            }
            TaskItem.UserID = userId.Value; //key player 
            _context.Entry(existingTaskItem).CurrentValues.SetValues(TaskItem);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(TaskItem.TaskID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.TaskID == id);
        }
    }
}
