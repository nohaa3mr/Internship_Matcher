namespace InternshipMatcher.Application.Features.Internships.ViewModels;


    public class AddInternshipRequestViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> SkillIDs { get; set; } = new List<Guid>();
    }

