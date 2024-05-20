using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Add this namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todolist.Data;
using todolist.Models;

namespace todolist.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }
            TaskItem.UserID = userId.Value;
            TaskItem.DateCreated = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _context.TaskItems.Add(TaskItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
