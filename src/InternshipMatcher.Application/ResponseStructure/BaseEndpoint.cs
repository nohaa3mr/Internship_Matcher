using FluentValidation;
using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class BaseEndpoint<TRequest, TResponse> : ControllerBase where TRequest : class  , new() where TResponse : class
{
    protected readonly IMediator Mediator;
    protected readonly IJWTService JwtService;

    public BaseEndpoint(BaseEndpointParameters parameters)
    {
        Mediator = parameters.Mediator;
        JwtService = parameters.JwtService;
    }
}
