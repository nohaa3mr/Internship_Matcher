using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternshipMatcher.Application.Features.Recruiters.ViewModels;
    public class CreateRecruiterProfileRequestViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(1000)]
        public string CompanyDescription { get; set; }

        [Required]
        [Url]
        public string CompanyWebsite { get; set; }

        [MaxLength(100)]
        public string? Position { get; set; }
    }
