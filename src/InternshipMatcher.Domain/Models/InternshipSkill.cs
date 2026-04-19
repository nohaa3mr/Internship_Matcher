namespace InternshipMatcher.Domain.Models
{
    public class InternshipSkill : BaseModel
    {
        public Internship Internship { get; set; }
        public Guid InternshipID { get; set; }
        public Guid SkillID { get; set; }
        public Skill Skill { get; set; }
    
    }
}