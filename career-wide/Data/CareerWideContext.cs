using career_wide.Models;
using career_wide.Models.Job;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace career_wide.Data;

public class CareerWideContext : IdentityDbContext<ApplicationUser>
{
    public CareerWideContext(
        DbContextOptions<CareerWideContext> options)
        : base(options)
    {
    }

    public DbSet<JobListing> JobListings { get; set; }
        = null!;
    public DbSet<JobApplication> JobApplications { get; set; }
    = null!;
}