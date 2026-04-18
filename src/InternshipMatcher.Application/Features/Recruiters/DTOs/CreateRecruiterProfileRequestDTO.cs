namespace InternshipMatcher.Application.Features.Recruiters.DTOs
{
    public class CreateRecruiterProfileRequestDTO
    {
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyWebsite { get; set; }
        public string? Position { get; set; }
    }
}