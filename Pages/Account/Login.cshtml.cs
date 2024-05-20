using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using todolist.Data;
using todolist.Models;
using BCrypt.Net;

namespace todolist.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("UserID", user.UserID);
                return RedirectToPage("/Tasks/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
