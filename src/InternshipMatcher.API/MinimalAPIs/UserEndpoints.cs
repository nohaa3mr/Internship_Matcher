using InternshipMatcher.API.Common.CommonViewModels;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Users.CreateUserOrchestrotor;
using InternshipMatcher.Application.Features.Users.DTOs;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.MinimalAPIs
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/regiseration", async (IMediator mediator , [FromBody] Result<UserRegisterationReqVM> request) =>
            {
                var DTO = request.Adapt<UserRegisterationRequestDTO>();
                var Request = mediator.Send(new CreateUserOrchestrator(DTO));
                Result<RegisterationResponseViewModel>.Success(Request.Adapt<Result<RegisterationResponseViewModel>>().Message) ;
            });
       
            return group;
        }

    }
   
}
