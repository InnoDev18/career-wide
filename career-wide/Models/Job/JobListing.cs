using System.ComponentModel.DataAnnotations;

namespace career_wide.Models.Job;

public class JobListing
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Location { get; set; } = string.Empty;

    [Required, StringLength(40)]
    public string EmploymentType { get; set; } = string.Empty;

    [Required, StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    public string? SalaryRange { get; set; }

    public DateTime PostedOn { get; set; } = DateTime.UtcNow;

    [Required, DataType(DataType.Date)]
    public DateTime ClosingDate { get; set; }

    public bool IsPublished { get; set; } = true;

    public ICollection<JobApplication> Applications { get; set; }
    = new List<JobApplication>();
}