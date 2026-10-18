using System.ComponentModel.DataAnnotations;

namespace career_wide.Models.Job;

public class JobListing
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(120)]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Location { get; set; } = string.Empty;

    [Required, StringLength(40)]
    public string EmploymentType { get; set; } = string.Empty;

    [Required, StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Salary Range")]
    public string? SalaryRange { get; set; }

    [Display(Name = "Posted On")]
    public DateTime PostedOn { get; set; } = DateTime.UtcNow;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Closing Date")]
    public DateTime ClosingDate { get; set; }

    [Display(Name = "Is Published")]
    public bool IsPublished { get; set; } = true;

    public ICollection<JobApplication> Applications { get; set; }
    = new List<JobApplication>();
}