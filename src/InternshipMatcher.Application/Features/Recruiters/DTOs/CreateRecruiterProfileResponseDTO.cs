namespace InternshipMatcher.Application.Features.Recruiters.DTOs
{
    public class CreateRecruiterProfileResponseDTO
    {
        public Guid ID { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyLogoPath { get; set; }
        public string? Position { get; set; }
        public Guid UserID { get; set; }
    }
}