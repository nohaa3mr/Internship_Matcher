namespace InternshipMatcher.Application.Features.Applications.DTOs
{
    public class StudentApplyToInternshipResponseDTO
    {
        public Guid ApplicationID { get; set; }

        public int MatchScore { get; set; }
        public string Reasoning { get; set; } = string.Empty;
    }
}