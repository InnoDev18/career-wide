
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using career_wide.Models.Job;
using career_wide.Data;
using Microsoft.AspNetCore.Authorization;



[Authorize(Roles = "Admin")]
public class JobListingsController : Controller
{
    private readonly CareerWideContext _context;

    public JobListingsController(CareerWideContext context)
    {
        _context = context;
    }

    // GET: JOBLISTINGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.JobListings.ToListAsync());
    }

    // GET: JOBLISTINGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joblisting = await _context.JobListings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (joblisting == null)
        {
            return NotFound();
        }

        return View(joblisting);
    }

    // GET: JOBLISTINGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: JOBLISTINGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,CompanyName,Location,EmploymentType,Description,SalaryRange,PostedOn,ClosingDate,IsPublished")] JobListing joblisting)
    {
        if (ModelState.IsValid)
        {
            joblisting.PostedOn = DateTime.UtcNow;
            _context.Add(joblisting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(joblisting);
    }

    // GET: JOBLISTINGS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joblisting = await _context.JobListings.FindAsync(id);
        if (joblisting == null)
        {
            return NotFound();
        }
        return View(joblisting);
    }

    // POST: JOBLISTINGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,CompanyName,Location,EmploymentType,Description,SalaryRange,PostedOn,ClosingDate,IsPublished")] JobListing joblisting)
    {
        if (id != joblisting.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(joblisting);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobListingExists(joblisting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(joblisting);
    }

    // GET: JOBLISTINGS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joblisting = await _context.JobListings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (joblisting == null)
        {
            return NotFound();
        }

        return View(joblisting);
    }

    // POST: JOBLISTINGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var joblisting = await _context.JobListings.FindAsync(id);
        if (joblisting != null)
        {
            _context.JobListings.Remove(joblisting);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JobListingExists(int? id)
    {
        return _context.JobListings.Any(e => e.Id == id);
    }
}
