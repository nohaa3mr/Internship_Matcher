using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Applications.DTOs
{
   public class StudentApplyToInternshipRequestDTO
        {
            public Guid StudentProfileID { get; set; }
            public Guid InternshipID { get; set; }
            public string? CoverLetter { get; set; }
            public List<string> Skills { get; set; }
    }
}
