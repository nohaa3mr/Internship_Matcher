namespace InternshipMatcher.Domain.Models;

public class StudentProfile : BaseModel
{
    public string FullName { get; set; }
    public User User { get; set; }
    public Guid UserID { get; set; }
    public string Bio { get; set; }
    public string University { get; set; }= string.Empty;
    public string CVPath { get; set; }
    public ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
}
