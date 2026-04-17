using InternshipMatcher.Application.Features.Internships.Commands.AddInternship;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Features.Internships.ViewModels;
using Mapster;
using MediatR;

namespace InternshipMatcher.API.MinimalAPIs;

public static class InternshipsEndpoints
{
    public static RouteGroupBuilder MapInternships(this RouteGroupBuilder group)
    {
        group.MapPost("", async (AddInternshipRequestViewModel vm, IMediator mediator) =>
        {
            var dto = vm.Adapt<AddInternshipRequestDTO>();
            var result = await mediator.Send(new AddInternshipCommand(dto));

            if (!result.IsSuccess)
                return Results.BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });

            return Results.Created($"/internships/{result?.Data?.ID}",
                result.Data.Adapt<AddInternshipResponseViewModel>());
        })
        .WithName("AddInternship")
        .RequireAuthorization()
        .Produces<AddInternshipResponseViewModel>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}
