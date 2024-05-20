using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using todolist.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace todolist.Pages.Tasks
{
    public class ExportModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExportModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var tasks = await _context.TaskItems
                .Where(t => t.UserID == userId)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tasks");
                worksheet.Cells[1, 1].Value = "Task ID";
                worksheet.Cells[1, 2].Value = "Title";
                worksheet.Cells[1, 3].Value = "Description";
                worksheet.Cells[1, 4].Value = "Is Completed";
                worksheet.Cells[1, 5].Value = "Due Date";

                for (int i = 0; i < tasks.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = tasks[i].TaskID;
                    worksheet.Cells[i + 2, 2].Value = tasks[i].Title;
                    worksheet.Cells[i + 2, 3].Value = tasks[i].Description;
                    worksheet.Cells[i + 2, 4].Value = tasks[i].IsCompleted ? "Yes" : "No";
                    worksheet.Cells[i + 2, 5].Value = tasks[i].DueDate.ToShortDateString();
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var fileName = $"Tasks_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
