using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.API.MinimalAPIs;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Internships.Queries
{
    public sealed record GetInternshipsQuery : IRequest<Result<List<InternshipDTO>>>;
    public class GetInternshipsQueryHandler : IRequestHandler<GetInternshipsQuery, Result<List<InternshipDTO>>>
    {
        private readonly IGeneralRepository<Internship> _internshipRepository;
        public GetInternshipsQueryHandler(IGeneralRepository<Internship> internshipRepository)
        {
            _internshipRepository = internshipRepository;
        }
        public async Task<Result<List<InternshipDTO>>> Handle(GetInternshipsQuery request, CancellationToken cancellationToken)
        {
            var internships = await _internshipRepository.GetPageAsync();
            var internshipDTOs = internships.Select(i => new InternshipDTO
            {
                ID = i.ID,
                Title = i.Title,
                Description = i.Description,
                Location = i.Location,
                CompanyName = i.CompanyName,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                PostedAt = i.PostedAt
            }).ToList();
            return Result<List<InternshipDTO>>.Success(internshipDTOs);
        }
    }

}
