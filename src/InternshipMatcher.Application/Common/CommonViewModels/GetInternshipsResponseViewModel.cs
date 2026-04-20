using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Common.CommonViewModels;

public class GetInternshipsResponseViewModel
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime PostedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CompanyName { get; set; }
}
