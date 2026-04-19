using InternshipMatcher.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IAIService
    {
        public  Task<MatchResult> GetMatchScoreAsync(string Skills , string Description);
    }
}
