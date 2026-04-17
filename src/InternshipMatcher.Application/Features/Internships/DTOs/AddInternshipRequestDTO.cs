namespace InternshipMatcher.Application.Features.Internships.DTOs
{
    public class AddInternshipRequestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RecruiterProfileID { get; set; }
        public List<int> SkillIDs { get; set; } = new List<int>();
    }
}