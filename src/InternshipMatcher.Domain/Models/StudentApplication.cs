namespace InternshipMatcher.Domain.Models;

public class StudentApplication : BaseModel
{
    public Guid UserID { get; set; }
    public Guid StudentProfileID { get; set; }
    public Guid InternshipID { get; set; }
    public Guid ApplicationFormID { get; set; }
    public string Skills { get; set; } 
    public DateTime AppliedAt { get; set; }
}
