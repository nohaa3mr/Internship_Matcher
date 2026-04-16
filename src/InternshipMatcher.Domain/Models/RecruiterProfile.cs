using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Domain.Models;

public class RecruiterProfile : BaseModel
{
    public string FullName { get; set; }
    public User User { get; set; }
    public Guid UserID { get; set; }
    public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyLogoPath { get; set; } = string.Empty;
    public string? Position { get; set; }


}
