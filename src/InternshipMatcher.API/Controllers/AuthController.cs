using InternshipMatcher.API.Common.CommonViewModels;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseEndpoint<UserRegisterationReqVM, RegisterationResponseViewModel>
{
    private readonly IGeneralRepository<User> _repository;

    public AuthController(BaseEndpointParameters parameters, IGeneralRepository<User> repository ) : base(parameters)
    {
        this._repository = repository;
    }
    [HttpPost("Registeration")]
    public async Task<Result<RegisterationResponseViewModel>> RegisterUser(UserRegisterationReqVM user)
    {
        var User = user.Adapt<User>();
        if (User == null)
        {
            return Result<RegisterationResponseViewModel>.Failure("Invalid user data");
        }
        var addedUser = await _repository.AddAsync(User);
        if(!addedUser)
        {
            return Result<RegisterationResponseViewModel>.Failure("Failed to register user");
        }
        var token = await JwtService.GenerateToken(user.ID, user.Email);
        return Result<RegisterationResponseViewModel>.Success(new RegisterationResponseViewModel
        {
            Token = token,
            UserName = user.Email.Split('@')[0]
        });
    }
    [HttpPost("Login")]

    public async Task<Result<UserLoginResponseViewModel>> LoginUser(UserLoginRequestViewModel user)
    {
        var User = user.Adapt<User>();
       var existingUser = await _repository.GetByPredicateAsync(u => u.Email == User.Email && u.Password == User.Password);
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
