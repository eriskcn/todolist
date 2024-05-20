using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using todolist.Data;
using todolist.Models;

namespace todolist.Pages.Tasks
{
    public class ImportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ImportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }

        public List<string> ImportResult { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                ModelState.AddModelError("ExcelFile", "Please upload a valid Excel file.");
                return Page();
            }

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var importResult = new List<string>();
            using (var stream = new MemoryStream())
            {
                await ExcelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var title = worksheet.Cells[row, 1].Text;
                            var description = worksheet.Cells[row, 2].Text;
                            var dueDateText = worksheet.Cells[row, 3].Text;
                            DateTime dueDate;
                            if (!DateTime.TryParse(dueDateText, out dueDate))
                            {
                                importResult.Add($"Row {row}: Invalid due date format.");
                                continue;
                            }

                            var taskItem = new TaskItem
                            {
                                UserID = userId.Value,
                                Title = title,
                                Description = description,
                                DueDate = dueDate,
                                DateCreated = DateTime.Now
                            };

                            _context.TaskItems.Add(taskItem);
                            importResult.Add($"Row {row}: Task '{title}' imported successfully.");
                        }
                        catch (Exception ex)
                        {
                            importResult.Add($"Row {row}: Error - {ex.Message}");
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            ImportResult = importResult;
            return Page();
        }
    }
}
