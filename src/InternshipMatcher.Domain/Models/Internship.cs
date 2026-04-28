using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Domain.Models
{
    public class Internship: BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RecruiterProfileID { get; set; }
        public string CompanyName { get; set; }
        public RecruiterProfile RecruiterProfile { get; set; }
        public ICollection<ApplicationForm> Applications { get; set; } = new HashSet<ApplicationForm>();
        public ICollection<InternshipSkill> InternshipSkills { get; set; } = new List<InternshipSkill>();
        public int ApplicantsCount { get; set; }
    }
}
