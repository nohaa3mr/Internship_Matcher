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

namespace InternshipMatcher.Application.Features.Users.CreateUserOrchestrotor
{
    public sealed record CreateUserOrchestrator(UserRegisterationRequestDTO DTO) : IRequest<Result<UserRegisterationResponseDTO>>;
    public class CreateUserOrchestratorHandler : IRequestHandler<CreateUserOrchestrator, Result<UserRegisterationResponseDTO>>
    {
        private readonly IGeneralRepository<User> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IJWTService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserOrchestratorHandler(IGeneralRepository<User> repository , IUnitOfWork unitOfWork, IMediator mediator, IPasswordHasher passwordHasher, IJWTService jwtService)
        {
            _repository = repository;
            this._unitOfWork = unitOfWork;
            this._mediator = mediator;
            this._passwordHasher = passwordHasher;
            this._jwtService = jwtService;
        }
    
           public async Task<Result<UserRegisterationResponseDTO>> Handle(
    CreateUserOrchestrator request,
    CancellationToken cancellationToken)
        {
            // 1. Check duplicate
            var exists = await _repository.IsExist(u => u.Email == request.DTO.Email);
            if (exists)
                return Result<UserRegisterationResponseDTO>.Failure("Email already in use");

            // 2. Create user
            var user = new User
            {
                ID = Guid.NewGuid(),
                FirstName = request.DTO.FirstName,
                LastName = request.DTO.LastName,
                Email = request.DTO.Email,
                Password = _passwordHasher.Hash(request.DTO.Password),
                Role = Enum.Parse<UserRole>(request.DTO.Role, true),
                RefreshToken =  _passwordHasher.Hash(await _jwtService.GenerateRefreshToken())
            };

            // 3. Add user FIRST
            await _repository.AddAsync(user);

            // 4. Create profile BASED ON ROLE
            if (user.Role == UserRole.Recruiter)
            {
                var recruiterDto = new CreateRecruiterProfileRequestDTO
                {
                    UserID = user.ID,
                    FullName = $"{user.FirstName} {user.LastName}",
                    CompanyDescription = "",
                     CompanyName = "",
                     CompanyWebsite = "",
                     Position = ""

                };

                var result = await _mediator.Send(new CreateRecruiterProfileCommand(recruiterDto));

                if (!result.IsSuccess)
                    return Result<UserRegisterationResponseDTO>.Failure("Failed to create recruiter profile");
            }
            else if (user.Role == UserRole.Student)
            {
                var studentDto = new CreateStudentProfileRequestDTO
                {
                    UserID = user.ID,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Bio = "",
                    CVPath = "",
                    University = "",
                    StudentSkills = new List<StudentSkillDTO>() {}
                };

                var result = await _mediator.Send(new CreateStudentProfileCommand(studentDto));

                if (!result.IsSuccess)
                    return Result<UserRegisterationResponseDTO>.Failure("Failed to create student profile");
            }
            await _unitOfWork.SaveChangesAsync();
            // 5. Return response
            return Result<UserRegisterationResponseDTO>.Success(
                new UserRegisterationResponseDTO
                {
                    ID = user.ID,
                    Email = user.Email,
                    UserRole = user.Role.ToString(),
                    UserName = $"{user.FirstName} {user.LastName}",
                    refreshToken = user.RefreshToken,
                    accessToken = await _jwtService.GenerateToken(user.ID, user.Email),
                    IsRegistered = true
                },
                "User registered successfully"
            );
        }
    }
    
}
