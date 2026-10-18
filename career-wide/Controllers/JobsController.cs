using career_wide.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using career_wide.Models.Job;

namespace career_wide.Controllers;

public class JobsController : Controller
{
    private readonly CareerWideContext _context;
    private readonly IWebHostEnvironment _environment;

    public JobsController(CareerWideContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var jobs = await _context.JobListings
            .Where(job => job.IsPublished && job.ClosingDate >= today)
            .OrderBy(job => job.ClosingDate)
            .ToListAsync();

        return View(jobs);
    }
    [HttpGet]
    public async Task<IActionResult> Apply(int id)
    {
        var today = DateTime.Today;

        var job = await _context.JobListings
            .FirstOrDefaultAsync(job =>
                job.Id == id &&
                job.IsPublished &&
                job.ClosingDate >= today);

        if (job is null)
        {
            return NotFound();
        }

        ViewData["JobTitle"] = job.Title;

        return View(new JobApplication
        {
            JobListingId = job.Id
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(
    int id,
    [Bind("ApplicantName,Email,CoverLetter")] JobApplication application,
    IFormFile? cvFile)
    {
        var today = DateTime.Today;

        var job = await _context.JobListings
            .FirstOrDefaultAsync(job =>
                job.Id == id &&
                job.IsPublished &&
                job.ClosingDate >= today);

        if (job is null)
        {
            return NotFound();
        }
        if (!string.IsNullOrWhiteSpace(application.Email))
        {
            application.Email = application.Email.Trim();
            var emailToCheck = application.Email.ToUpper();

            var alreadyApplied = await _context.JobApplications
                .AnyAsync(existing =>
                    existing.JobListingId == job.Id &&
                    existing.Email.ToUpper() == emailToCheck);

            if (alreadyApplied)
            {
                ModelState.AddModelError(
                    nameof(application.Email),
                    "You have already applied for this job with this email address.");
            }
        }
        string? fileExtension = null;

        if (cvFile is not null)
        {
            fileExtension = Path.GetExtension(cvFile.FileName).ToLowerInvariant();

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("cvFile", "Please upload a PDF or Word document.");
            }

            if (cvFile.Length == 0 || cvFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("cvFile", "The CV must be smaller than 5 MB.");
            }
        }

        if (!ModelState.IsValid)
        {
            ViewData["JobTitle"] = job.Title;
            application.JobListingId = job.Id;
            return View(application);
        }

        application.JobListingId = job.Id;
        application.AppliedOn = DateTime.UtcNow;


        if (cvFile is not null && fileExtension is not null)
        {
            var uploadDirectory = Path.Combine(
                _environment.ContentRootPath,
                "App_Data",
                "ApplicantCvs");

            Directory.CreateDirectory(uploadDirectory);

            var storageName = $"{Guid.NewGuid():N}{fileExtension}";
            var filePath = Path.Combine(uploadDirectory, storageName);

            await using (var stream = new FileStream(filePath, FileMode.CreateNew))
            {
                await cvFile.CopyToAsync(stream);
            }

            application.CvFileName = Path.GetFileName(cvFile.FileName);
            application.CvStorageName = storageName;
        }

        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();

        return View("Submitted", job);
    }
}