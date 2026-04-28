using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.DTO
{
    public class MatchResult
    {
        public int Score { get; set; }
         public List<string> MissingSkills { get; set; } = new List<string>();
        public string Reasoning { get; internal set; }
    }
}
