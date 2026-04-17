namespace  InternshipMatcher.Application.Features.Internships.ViewModels;

public class AddInternshipResponseViewModel
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Skills { get; set; } = new List<string>();
}
