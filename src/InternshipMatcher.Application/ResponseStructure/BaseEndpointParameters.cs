using FluentValidation;
using InternshipMatcher.Application.Interfaces;
using MediatR;

namespace InternshipMatcher.API.Common.ResponseStructure
{
    public class BaseEndpointParameters
    {
        public IMediator Mediator { get; }
        public IJWTService JwtService { get; }

        public BaseEndpointParameters(
            IMediator mediator,
            IJWTService jwtService)
        {
            Mediator = mediator;
            JwtService = jwtService;
        }
    }
}