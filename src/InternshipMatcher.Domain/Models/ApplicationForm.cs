using InternshipMatcher.Domain.Enums;
namespace InternshipMatcher.Domain.Models;

public class ApplicationForm :BaseModel
{
    public StudentProfile StudentProfile { get; set; }
    public Guid StudentProfileID { get; set; }
    public Guid RecruiterProfileID { get; set; }
    public Internship Internship { get; set; }
    public Guid InternshipID { get; set; }
    public string? CoverLetter { get; set; }
    public DateTime AppliedAt { get; set; }
    public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Pending;
    public double MatchScore { get; set; } 


}