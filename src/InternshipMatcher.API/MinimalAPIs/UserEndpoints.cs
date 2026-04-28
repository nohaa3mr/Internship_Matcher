using InternshipMatcher.API.Common.CommonViewModels;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Users.CreateUserOrchestrotor;
using InternshipMatcher.Application.Features.Users.DTOs;
using InternshipMatcher.Application.Features.Users.RefreshToken;
using InternshipMatcher.Application.Features.Users.UserLogin;
using InternshipMatcher.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.MinimalAPIs
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/register", async (IMediator mediator , [FromBody] UserRegisterationReqVM request) =>
            {
                var DTO = request.Adapt<UserRegisterationRequestDTO>();
                
                var Request = await mediator.Send(new CreateUserOrchestrator(DTO));
               
                return Result<RegisterationResponseViewModel>.Success(Request.Data.Adapt<RegisterationResponseViewModel>());
            });

            group.MapPost("/login", async (IMediator mediator, [FromBody] UserLoginRequestViewModel model) =>
            {
                var RequestDTO = model.Adapt<UserLoginRequestDTO>();
                var Request = mediator.Send(new UserLoginCommand(RequestDTO));
                var Response = Request.Adapt<UserLoginResponseViewModel>();
                return Result<UserLoginResponseViewModel>.Success(Response);

            });
            group.MapPost("/refresh-token", async (IMediator mediator, [FromBody] TokenRefreshRequestViewModel model) =>
            {
               
                var Request = mediator.Send(new RefreshTokenCommand(model.RefreshToken));
                var Response = Request.Result.Data.Adapt< TokenRefreshResponseViewModel> ();
                return Result<TokenRefreshResponseViewModel>.Success(Response);
            });    

            return group;
        }

    }
   
}
