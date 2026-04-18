using InternshipMatcher.API.Common.CommonViewModels;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application;
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

    public AuthController(BaseEndpointParameters parameters, IGeneralRepository<User> repository  
        ,IPasswordHasher  passwordHasher, IEmailHasher emailHasher, IJWTService jwtService) : base(parameters)
    {
        this._repository = repository;
        this._passwordHasher = passwordHasher;
        this._emailHasher = emailHasher;
        this._jwtService = jwtService;
    }

    [HttpPost("registration")]
    public async Task<Result<RegisterationResponseViewModel>> RegisterUser(UserRegisterationReqVM user)
    {
        if (user == null)
            return Result<RegisterationResponseViewModel>.Failure("Invalid user data");

        var (passwordHash, passwordSalt) = _passwordHasher.Hash(user.Password);

        var newUser = new User
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.PhoneNumber,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = user.Role switch
            {
                "Recruiter" => UserRole.Recruiter,
                "Student" => UserRole.Student,
                _ => throw new ArgumentException("Invalid role")
            }
        };

        var addedUser = await _repository.AddAsync(newUser);
        if (addedUser == null)
            return Result<RegisterationResponseViewModel>.Failure("Failed to register user");

        var token = await _jwtService.GenerateToken(addedUser.ID, addedUser.Email);

        return Result<RegisterationResponseViewModel>.Success(new RegisterationResponseViewModel
        {
            Token = token,
            UserName = addedUser.Email.Split('@')[0]
        });
    }
    [HttpPost("Login")]

    public async Task<Result<UserLoginResponseViewModel>> LoginUser(UserLoginRequestViewModel user)
    {
        var User = user.Adapt<User>();
       var existingUser = await _repository.GetByPredicateAsync(u => u.Email == User.Email &&  _passwordHasher.Verify(user.Password, u.PasswordHash, u.PasswordSalt)==true);
        if (existingUser == null)
        {
            return Result<UserLoginResponseViewModel>.Failure("Invalid email or password");
        }
        var token = await JwtService.GenerateToken(user.ID, user.Email);
        return Result<UserLoginResponseViewModel>.Success(new UserLoginResponseViewModel
        {
            Token = token,
            Email = user.Email ,
            Password = user.Password,
            RefreshToken = await JwtService.GenerateRefreshToken(),
        });
    }

}
