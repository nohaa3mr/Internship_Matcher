using InternshipMatcher.Domain.Models;

namespace InternshipMatcher.Application.Features.Students.DTOs
{
    public class CreateStudentProfileRequestDTO
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string University { get; set; } = string.Empty;
        public string CVPath { get; set; }
        public ICollection<StudentSkillDTO> StudentSkills { get; set; } = new List<StudentSkillDTO>();
    }
}
