using InternshipMatcher.Application.Features.Applications.DTOs;
using InternshipMatcher.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Applications.Commands;

public sealed record StudentApplyToInternshipCommand(StudentApplyToInternshipDTO request) : IRequest<bool>;
public class StudentApplyToInternshipCommandHandler : IRequestHandler<StudentApplyToInternshipCommand, bool>
{
    public Task<bool> Handle(StudentApplyToInternshipCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

