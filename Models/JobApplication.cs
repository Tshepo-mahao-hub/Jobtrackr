using System.ComponentModel.DataAnnotations;

namespace JobTrackr.Models;

public enum ApplicationStatus { Wishlist, Applied, Interviewing, Offer, Rejected, Withdrawn }

public class JobApplication
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    [Required, StringLength(100)] public string CompanyName { get; set; } = string.Empty;
    [Required, StringLength(120), Display(Name = "Job title")] public string JobTitle { get; set; } = string.Empty;
    [StringLength(100)] public string? Location { get; set; }
    [Display(Name = "Salary / range")] public string? Salary { get; set; }
    [DataType(DataType.Date), Display(Name = "Application date")] public DateTime ApplicationDate { get; set; } = DateTime.Today;
    [DataType(DataType.Date), Display(Name = "Closing date")] public DateTime? ClosingDate { get; set; }
    [Display(Name = "Job description")] public string? JobDescription { get; set; }
    [Display(Name = "Recruiter name")] public string? RecruiterName { get; set; }
    [EmailAddress, Display(Name = "Recruiter email")] public string? RecruiterEmail { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    public ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
}

public class Interview
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public JobApplication? JobApplication { get; set; }
    [Required, Display(Name = "Interview date & time")] public DateTime ScheduledAt { get; set; }
    [Required, StringLength(50)] public string Type { get; set; } = "Video";
    public string? Notes { get; set; }
}

public class ApplicationDocument
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public JobApplication? JobApplication { get; set; }
    [Required] public string OriginalFileName { get; set; } = string.Empty;
    [Required] public string StoredFileName { get; set; } = string.Empty;
    [Required] public string DocumentType { get; set; } = "CV";
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
