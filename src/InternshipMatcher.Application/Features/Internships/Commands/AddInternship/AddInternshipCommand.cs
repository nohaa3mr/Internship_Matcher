using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Internships.Commands.AddInternship
{
    public sealed record AddInternshipCommand(AddInternshipRequestDTO DTO) : IRequest<Result<AddInternshipResponseDTO>>;
    public class AddInternshipCommandHandler : IRequestHandler<AddInternshipCommand, Result<AddInternshipResponseDTO>>
    {
        private readonly IGeneralRepository<Internship> _repository;

        public AddInternshipCommandHandler(IGeneralRepository<Internship> repository)
        {
            this._repository = repository;
        }

        public async Task<Result<AddInternshipResponseDTO>> Handle(AddInternshipCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result<AddInternshipResponseDTO>.Failure("Request cannot be null", errors: new List<string> { "Request cannot be null" });
            }
            var internship = request.DTO.Adapt<Internship>();
             var addedIndternship = await _repository.AddAsync(internship);
            if(addedIndternship )
            {    var responseDTO = internship.Adapt<AddInternshipResponseDTO>();
                return Result<AddInternshipResponseDTO>.Success(responseDTO, "Internship added successfully");
            }
            
            else
            {
                return Result<AddInternshipResponseDTO>.Failure("Failed to add internship", errors: new List<string> { "Failed to add internship" });
            }
        }
    }

}
