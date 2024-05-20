using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using todolist.Data;
using todolist.Models;
using BCrypt.Net;

namespace todolist.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

                var user = new User
                {
                    Username = Username,
                    Password = hashedPassword,
                    Email = Email,
                    DateCreated = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
