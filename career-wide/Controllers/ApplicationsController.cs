using career_wide.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace career_wide.Controllers;

[Authorize(Roles = "Admin")]
public class ApplicationsController : Controller
{
    private readonly CareerWideContext _context;
    private readonly IWebHostEnvironment _environment;

    public ApplicationsController(CareerWideContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _context.JobApplications
            .Include(application => application.JobListing)
            .OrderByDescending(application => application.AppliedOn)
            .ToListAsync();

        return View(applications);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DownloadCv(int id)
    {
        var application = await _context.JobApplications
            .FirstOrDefaultAsync(application => application.Id == id);

        if (application is null ||
            string.IsNullOrWhiteSpace(application.CvStorageName) ||
            string.IsNullOrWhiteSpace(application.CvFileName) ||
            Path.GetFileName(application.CvStorageName) != application.CvStorageName)
        {
            return NotFound();
        }

        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "App_Data",
            "ApplicantCvs",
            application.CvStorageName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var contentType = Path.GetExtension(application.CvFileName).ToLowerInvariant() switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };

        return PhysicalFile(
            filePath,
            contentType,
            Path.GetFileName(application.CvFileName));
    }
}