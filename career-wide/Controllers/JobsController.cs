using career_wide.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using career_wide.Models.Job;

namespace career_wide.Controllers;

public class JobsController : Controller
{
    private readonly CareerWideContext _context;

    public JobsController(CareerWideContext context)
    {
        _context = context;
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
        [Bind("ApplicantName,Email,CoverLetter")] JobApplication application)
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

        if (!ModelState.IsValid)
        {
            ViewData["JobTitle"] = job.Title;
            application.JobListingId = job.Id;
            return View(application);
        }

        application.JobListingId = job.Id;
        application.AppliedOn = DateTime.UtcNow;

        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();

        return View("Submitted", job);
    }
}