using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.DTO
{
    public class MatchResult
    {
        public double Score { get; set; }
            public string Explanation { get; set; } = string.Empty;
     public List<string> MissingSkills { get; set; } = new List<string>();

    

    }
}
