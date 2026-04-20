using InternshipMatcher.Application.Features.Internships.Commands.AddInternship;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Features.Internships.Queries;
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
        .WithName("addInternship")
        .RequireAuthorization()
        .Produces<AddInternshipResponseViewModel>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/internships", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetInternshipsQuery());
            if (!result.IsSuccess)
                return Results.BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });

            return Results.Ok(result.Data.Adapt<List<InternshipDTO>>());
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetInternshipByIDQuery(id));
            if (!result.IsSuccess)
                return Results.BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            return Results.Ok(result.Data);
        }).WithName("getInternshipById");
        return group;
    }
}
