using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Recruiters.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text;
namespace InternshipMatcher.Application.Features.Recruiters.AddRecruiterCommand
{
    public sealed record CreateRecruiterProfileCommand(CreateRecruiterProfileRequestDTO DTO) : IRequest<Result<CreateRecruiterProfileResponseDTO>>;
    public class CreateRecruiterProfileCommandHandler: IRequestHandler<CreateRecruiterProfileCommand, Result<CreateRecruiterProfileResponseDTO>>
    {
        private readonly IGeneralRepository<RecruiterProfile> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public CreateRecruiterProfileCommandHandler(
            IGeneralRepository<RecruiterProfile> repository,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<Result<CreateRecruiterProfileResponseDTO>> Handle(
            CreateRecruiterProfileCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Request cannot be null");


            var idClaim = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idClaim))
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Unauthorized");

            if (!Guid.TryParse(idClaim, out var userID))
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Invalid user ID");

            var profile = request.DTO.Adapt<RecruiterProfile>();
            profile.UserID = userID;

     

            var added = await _repository.AddAsync(profile);
            if (added is null)
                return Result<CreateRecruiterProfileResponseDTO>.Failure("Failed to create recruiter profile");

            return Result<CreateRecruiterProfileResponseDTO>.Success(
                added.Adapt<CreateRecruiterProfileResponseDTO>(),
                "Recruiter profile created successfully");
        }
    }
}
