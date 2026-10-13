using career_wide.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace career_wide.Controllers;

[Authorize(Roles = "Admin")]
public class ApplicationsController : Controller
{
    private readonly CareerWideContext _context;

    public ApplicationsController(CareerWideContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _context.JobApplications
            .Include(application => application.JobListing)
            .OrderByDescending(application => application.AppliedOn)
            .ToListAsync();

        return View(applications);
    }
}