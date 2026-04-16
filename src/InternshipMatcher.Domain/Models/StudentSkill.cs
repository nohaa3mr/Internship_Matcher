namespace InternshipMatcher.Domain.Models
{
    public class StudentSkill :BaseModel
    {
        public StudentProfile  StudentProfile { get; set; }
        public Guid StudentProfileID { get; set; }

        public Guid SkillID { get; set; }
        public Skill Skill { get; set; }
    }
}