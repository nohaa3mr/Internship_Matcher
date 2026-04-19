using MediatR;
using  InternshipMatcher.Application.Features.Recruiters.ViewModels;
using Mapster;
using InternshipMatcher.Application.Features.Recruiters.DTOs;
using InternshipMatcher.Application.Features.Recruiters.AddRecruiterCommand;


namespace InternshipMatcher.API.MinimalAPIs
{
    public static class RecruitersEndpoints
    {
        public static RouteGroupBuilder MapRecruiters(this RouteGroupBuilder group)
        {
            group.MapPost("/Recruiters", async (CreateRecruiterProfileRequestViewModel viewModel, IMediator mediator) =>
            {
                var request = viewModel.Adapt<CreateRecruiterProfileRequestDTO>();
                var result = await mediator.Send(new CreateRecruiterProfileCommand(request));
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Errors);
                }
                return Results.Ok(result);


            }).RequireAuthorization()
            ;            return group;
        }
    }
}
