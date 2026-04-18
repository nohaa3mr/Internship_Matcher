using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Applications.DTOs
{
    public class StudentApplyToInternshipDTO
    {
        public Guid StudentProfileID { get; set; }
        public Guid InternshipID { get; set; }
        public object Description { get; internal set; }
    }
}
