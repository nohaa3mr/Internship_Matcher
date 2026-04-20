using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Recruiters.AddRecruiterCommand;
using InternshipMatcher.Application.Features.Recruiters.DTOs;
using InternshipMatcher.Application.Features.Students.Commands;
using InternshipMatcher.Application.Features.Students.DTOs;
using InternshipMatcher.Application.Features.Users.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Enums;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Users.CreateUserOrchestrotor
{
    public sealed record CreateUserOrchestrator(UserRegisterationRequestDTO DTO) : IRequest<Result<UserRegisterationResponseDTO>>;
    public class CreateUserOrchestratorHandler : IRequestHandler<CreateUserOrchestrator, Result<UserRegisterationResponseDTO>>
    {
        private readonly IGeneralRepository<User> _repository;
        private readonly IMediator _mediator;
        private readonly IJWTService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserOrchestratorHandler(IGeneralRepository<User> repository, IMediator mediator, IPasswordHasher passwordHasher, IJWTService jwtService)
        {
            _repository = repository;
            this._mediator = mediator;
            this._passwordHasher = passwordHasher;
            this._jwtService = jwtService;
        }
        public async Task<Result<UserRegisterationResponseDTO>> Handle(
      CreateUserOrchestrator request, CancellationToken cancellationToken)
        {
            // 1. Duplicate check
            var exists = await _repository.IsExist(u => u.Email == request.DTO.Email);
            if (exists)
                return Result<UserRegisterationResponseDTO>.Failure("Email already in use");

            // 2. Build user
            var user = new User
            {
                FirstName = request.DTO.FirstName,
                LastName = request.DTO.LastName,
                Email = request.DTO.Email,
                Password = _passwordHasher.Hash(request.DTO.Password),
                Role = Enum.Parse<UserRole>(request.DTO.Role),
                RefreshToken = await _jwtService.GenerateRefreshToken()
            };

            // 3. Create role profile
            if (user.Role == UserRole.Recruiter)
            {
                var result = await _mediator.Send(
                    new CreateRecruiterProfileCommand(user.Adapt<CreateRecruiterProfileRequestDTO>()));
                if (!result.IsSuccess)
                    return Result<UserRegisterationResponseDTO>.Failure("Failed to create recruiter profile");
            }
            else if (user.Role == UserRole.Student)
            {
                var result = await _mediator.Send(
                    new CreateStudentProfileCommand(user.Adapt<CreateStudentProfileRequestDTO>()));
                if (!result.IsSuccess)
                    return Result<UserRegisterationResponseDTO>.Failure("Failed to create student profile");
            }
            else
            {
                return Result<UserRegisterationResponseDTO>.Failure("Invalid user role");
            }

            // 4. Save user
            var createdUser = await _repository.AddAsync(user);
            if (createdUser is null)
                return Result<UserRegisterationResponseDTO>.Failure("Failed to create user");

            // 5. Return response
            return Result<UserRegisterationResponseDTO>.Success(new UserRegisterationResponseDTO
            {
                ID = createdUser.ID,
                Email = createdUser.Email,
                accessToken = await _jwtService.GenerateToken(createdUser.ID, createdUser.Email),
                refreshToken = createdUser.RefreshToken,
                IsRegistered = true,
                UserRole = createdUser.Role.ToString(),
                UserName = $"{createdUser.FirstName} {createdUser.LastName}"
            });
        }
    }
}
