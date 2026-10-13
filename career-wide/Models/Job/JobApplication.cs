using System.ComponentModel.DataAnnotations;

namespace career_wide.Models.Job;

public class JobApplication
{
    public int Id { get; set; }

    [Required]
    public int JobListingId { get; set; }

    public JobListing? JobListing { get; set; }

    [Required, StringLength(120)]
    public string ApplicantName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string CoverLetter { get; set; } = string.Empty;

    public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
}