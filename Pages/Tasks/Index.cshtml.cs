using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task OnGetAsync()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                RedirectToPage("/Account/Login"); 
                return;     
            }

            TaskItems = await _context.TaskItems
                .Where(t => t.UserID == userId)
                .ToListAsync();
        }

    }
}
