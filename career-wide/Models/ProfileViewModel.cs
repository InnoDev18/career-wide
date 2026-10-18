using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace career_wide.Models;

public class ProfileViewModel
{
    [Required, StringLength(100)]
    public string? FirstName { get; set; }

    [Required, StringLength(100)]
    public string? LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [StringLength(150)]
    public string? Location { get; set; }

    [StringLength(150)]
    public string? ProfessionalTitle { get; set; }

    [StringLength(2000)]
    public string? ProfessionalSummary { get; set; }

    [StringLength(1500)]
    public string? Skills { get; set; }

    [Url]
    public string? LinkedInUrl { get; set; }

    [Url]
    public string? PortfolioUrl { get; set; }

    public IFormFile? CvFile { get; set; }

    public string? CurrentCvFileName { get; set; }
}