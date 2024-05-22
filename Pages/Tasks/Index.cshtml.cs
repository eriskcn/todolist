using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using todolist.Data;
using todolist.Models;
using Microsoft.AspNetCore.Http;

namespace todolist.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<TaskItem> TaskItems { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login"); 
            }

            TaskItems = await _context.TaskItems
                .Where(t => t.UserID == userId)
                .Include(t => t.TaskFiles)
                .ToListAsync();

            return Page();
        }
    }
}
