using InternshipMatcher.API.Common.CommonViewModels;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Enums;
using InternshipMatcher.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseEndpoint<UserRegisterationReqVM, RegisterationResponseViewModel>
{
    private readonly IGeneralRepository<User> _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailHasher _emailHasher;
    private readonly IJWTService _jwtService;

    public AuthController(BaseEndpointParameters parameters, IGeneralRepository<User> repository ,IPasswordHasher  passwordHasher, IEmailHasher emailHasher, IJWTService jwtService) : base(parameters)
    {
      _repository = repository;
        _passwordHasher = passwordHasher;
        _emailHasher = emailHasher;
        _jwtService = jwtService;
    }

    [HttpPost("registration")]
    public async Task<Result<RegisterationResponseViewModel>> RegisterUser(UserRegisterationReqVM user)
    {
        if (user == null)
            return Result<RegisterationResponseViewModel>.Failure("Invalid user data");
            var existingUser = await _repository.GetByPredicateAsync(u => u.Email == user.Email);
        if(existingUser != null)
            return Result<RegisterationResponseViewModel>.Failure("User already exists");

        var passwordHash = _passwordHasher.Hash(user.Password);

        if (user.Role == UserRole.Student.ToString())
        {
            var newUser = new User
            {
                ID = Guid.NewGuid(),
                Email = user.Email,
                Password = passwordHash,
                Role = UserRole.Student,
                RefreshToken = await _jwtService.GenerateRefreshToken(),
                
            };

            var studentProfile = new StudentProfile
            {
                ID = Guid.NewGuid(),
                FullName = $"{user.FirstName} {user.LastName}",
                UserID = newUser.ID,  
                User = newUser,
            };

            var createdUser = await _repository.AddAsync(newUser);
            if(createdUser is null)
                return Result<RegisterationResponseViewModel>.Failure("Failed to create user");
            return Result<RegisterationResponseViewModel>.Success(new RegisterationResponseViewModel
            {
                ID = createdUser.ID,
                Email = createdUser.Email,
                accessToken = await _jwtService.GenerateToken(createdUser.ID, createdUser.Email),
                refreshToken = createdUser.RefreshToken,
                IsRegistered = true,
                UserRole = createdUser.Role.ToString(),
                UserName = studentProfile.FullName

            }, "User registered successfully");


        }
        else if (user.Role == UserRole.Recruiter.ToString())
        {
            var newUser = new User
            {
                ID = Guid.NewGuid(),
                Email = user.Email,
                Password = passwordHash,
                Role = UserRole.Recruiter,
                RefreshToken = await _jwtService.GenerateRefreshToken(),
            };
            var recruiterProfile = new RecruiterProfile
            {
                ID = Guid.NewGuid(),
                CompanyName = $"{user.FirstName} {user.LastName}",
                UserID = newUser.ID,  
                User = newUser,
            };
            var createdUser = await _repository.AddAsync(newUser);
            if(createdUser is null)
                return Result<RegisterationResponseViewModel>.Failure("Failed to create user");
            return Result<RegisterationResponseViewModel>.Success(new RegisterationResponseViewModel
            {
                ID = createdUser.ID,
                Email = createdUser.Email,
                accessToken = await _jwtService.GenerateToken(createdUser.ID, createdUser.Email),
                refreshToken = createdUser.RefreshToken,
                IsRegistered = true,
                UserRole = createdUser.Role.ToString(),
                UserName = recruiterProfile.FullName,  
            }, "User registered successfully");
        }
        else
        {
            return Result<RegisterationResponseViewModel>.Failure("Invalid user role");
        }




    }
    [HttpPost("login")]

    public async Task<Result<UserLoginResponseViewModel>> LoginUser(UserLoginRequestViewModel request)
    {
        var User = request.Adapt<User>();
       var existingUser = await _repository.GetByPredicateAsync(u => u.Email == User.Email &&  _passwordHasher.Verify(request.Password,User.Password));
        if (existingUser == null)
        {
            return Result<UserLoginResponseViewModel>.Failure("Invalid email or password");
        }
        var token = await JwtService.GenerateToken(request.ID, request.Email);
        return Result<UserLoginResponseViewModel>.Success(new UserLoginResponseViewModel
        {
            Token = token,
            Email = request.Email ,
            Password = request.Password,
            RefreshToken = await JwtService.GenerateRefreshToken(),
        });
    }

}
