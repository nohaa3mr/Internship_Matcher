using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Recruiters.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;

namespace InternshipMatcher.Application.Features.Recruiters.AddRecruiterCommand
{
    public sealed record CreateRecruiterProfileCommand(CreateRecruiterProfileRequestDTO DTO) : IRequest<Result<CreateRecruiterProfileResponseDTO>>;

    public class CreateRecruiterProfileCommandHandler : IRequestHandler<CreateRecruiterProfileCommand, Result<CreateRecruiterProfileResponseDTO>>
    {
        private readonly IGeneralRepository<RecruiterProfile> _repository;

        public CreateRecruiterProfileCommandHandler(IGeneralRepository<RecruiterProfile> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CreateRecruiterProfileResponseDTO>> Handle(
            CreateRecruiterProfileCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Request cannot be null");

            // Use the UserID explicitly provided in the DTO — works for both
            // registration (orchestrator supplies it) and authenticated standalone calls
            if (request.DTO.UserID == Guid.Empty)
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Invalid user ID");

            var profile = request.DTO.Adapt<RecruiterProfile>();
            profile.UserID = request.DTO.UserID;

           await _repository.AddAsync(profile);
          
            return Result<CreateRecruiterProfileResponseDTO>.Success(
                profile.Adapt<CreateRecruiterProfileResponseDTO>(),
                "Recruiter profile created successfully");
        }
    }
}