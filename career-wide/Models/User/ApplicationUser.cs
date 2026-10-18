using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace career_wide.Models;

public class ApplicationUser : IdentityUser
{
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
    public string? Location { get; set; }
    [Display(Name = "Professional Title")]
    public string? ProfessionalTitle { get; set; }
    [Display(Name = "Professional Summary")]
    public string? ProfessionalSummary { get; set; }
    public string? Skills { get; set; }
    [Display(Name = "LinkedIn Url")]
    public string? LinkedInUrl { get; set; }
    [Display(Name = "Portfolio Url")]
    public string? PortfolioUrl { get; set; }

    // The uploaded CV itself will be handled separately.
    public string? CvFileName { get; set; }
    public string? CvStorageName { get; set; }
    public DateTime? CvUploadedOn { get; set; }
}