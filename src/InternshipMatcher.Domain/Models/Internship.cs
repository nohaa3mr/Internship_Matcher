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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RecruiterProfileID { get; set; }
        public RecruiterProfile RecruiterProfile { get; set; }
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public ICollection<InternshipSkill> InternshipSkills { get; set; } = new List<InternshipSkill>();
    }
}
