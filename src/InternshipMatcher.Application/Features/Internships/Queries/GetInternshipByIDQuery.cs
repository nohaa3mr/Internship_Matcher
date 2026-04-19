using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Internships.Queries
{
    public sealed record GetInternshipByIDQuery(Guid ID) : IRequest<Result<InternshipDTO>>;
    public class GetInternshipByIDQueryHandler : IRequestHandler<GetInternshipByIDQuery, Result<InternshipDTO>>
    {
        private readonly IGeneralRepository<Internship> _repository;
        public GetInternshipByIDQueryHandler(IGeneralRepository<Internship> repository)
        {
            this._repository = repository;
        }
        public async Task<Result<InternshipDTO>> Handle(GetInternshipByIDQuery request, CancellationToken cancellationToken)
        {
            var internship = await _repository.GetByIdAsync(request.ID);
            if (internship is null)
                return Result<InternshipDTO>.Failure("Internship not found");
            var dto = internship.Adapt<InternshipDTO>();
            return Result<InternshipDTO>.Success(dto);
        }
    }
}