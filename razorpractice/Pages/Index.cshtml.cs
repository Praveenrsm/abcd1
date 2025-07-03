using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace razorpractice.Pages
{
    public class IndexModel : PageModel
    {
        // For displaying JSON or Content in the page
        public string JsonResult { get; set; }
        public string ContentResult { get; set; }

        // File Download handler
       
        public IActionResult OnGetDownload()
        {
            // Generate PDF into a memory stream
            var stream = new MemoryStream();

            // Build the PDF using QuestPDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text("Sample PDF Report");
                            col.Item().Text("Generated on: " + DateTime.Now);
                            col.Item().PaddingTop(10).LineHorizontal(1);
                            col.Item().Text("This is a PDF file generated on the fly using QuestPDF.");
                        });
                });
            }).GeneratePdf(stream);

            // Return the PDF as a downloadable file
            stream.Position = 0;
            return File(stream, "application/pdf", "generated.pdf");
        }

        // JSON handler
        public IActionResult OnGetJson()
        {
            var data = new { Name = "Alice", Age = 30 };
            // Instead of returning JSON directly, set JsonResult property to display on page
            JsonResult = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            return Page();

            // Or to return JSON as API response, use:
            // return new JsonResult(data);
        }

        // Content handler
        public IActionResult OnGetContent()
        {
            ContentResult = "This is plain text content from OnGetContent handler.";
            return Page();
        }

        // Redirect handler
        public IActionResult OnGetRedirect()
        {
            return RedirectToPage("/Privacy");
        }
    }
}
