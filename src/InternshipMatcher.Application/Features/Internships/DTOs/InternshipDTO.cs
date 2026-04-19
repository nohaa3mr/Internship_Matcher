namespace InternshipMatcher.Application.Features.Internships.DTOs
{
    public class InternshipDTO
    {
        public object ID { get; internal set; }
        public object Title { get; internal set; }
        public object Description { get; internal set; }
        public object Location { get; internal set; }
        public object StartDate { get; internal set; }
        public object EndDate { get; internal set; }
        public object RecruiterProfileID { get; internal set; }
        public object CompanyName { get; internal set; }
    }
}