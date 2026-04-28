namespace InternshipMatcher.API.MinimalAPIs
{
    public class UpdateInternshipRequestViewModel
    {
        public Guid ID { get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RecruiterProfileID { get; set; }
        public string CompanyName { get; set; }
    }
}