using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.DTO
{
    public class ApplicationDTO
    {
        public Guid ID {  get; set; }
        public Guid UserID { get; set; }
        public Guid InternshipID { get; set; }
        public int Score { get; set; } = 0;
        public string Explanation { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
