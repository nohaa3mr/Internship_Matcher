using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Users.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;

namespace InternshipMatcher.Application.Features.Users.UserLogin
{
    public sealed record UserLoginCommand(UserLoginRequestDTO DTO) : IRequest<Result<UserLoginResponseDTO>>;

    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Result<UserLoginResponseDTO>>
    {
        private readonly IGeneralRepository<User> _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTService _jwtService;

        public UserLoginCommandHandler(IGeneralRepository<User> repository, IPasswordHasher passwordHasher, IJWTService jwtService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<Result<UserLoginResponseDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch by email only — never filter by password hash inside the query
            var existingUser = await _repository.GetByPredicateAsync(u => u.Email == request.DTO.Email);
            if (existingUser == null || !_passwordHasher.Verify(request.DTO.Password, existingUser.Password))
                return Result<UserLoginResponseDTO>.Failure("Invalid email or password");

            // 2. Generate tokens using the real user ID from the database
            var accessToken = await _jwtService.GenerateToken(existingUser.ID, existingUser.Email);
            var refreshToken = await _jwtService.GenerateRefreshToken();

            // 3. Persist the new refresh token
            existingUser.RefreshToken = refreshToken;
            await _repository.UpdateAsync(existingUser);

            // 4. Return response — no password field
            return Result<UserLoginResponseDTO>.Success(new UserLoginResponseDTO
            {
                ID = existingUser.ID,
                Email = existingUser.Email,
                Token = accessToken,
                RefreshToken = refreshToken,
                UserRole = existingUser.Role.ToString(),
                UserName = $"{existingUser.FirstName} {existingUser.LastName}"
            });
        }
    }
}