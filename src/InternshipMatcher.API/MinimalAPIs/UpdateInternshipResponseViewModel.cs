namespace InternshipMatcher.API.MinimalAPIs
{
    internal class UpdateInternshipResponseViewModel
    {
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