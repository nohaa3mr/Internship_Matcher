using InternshipMatcher.Application.Features.Applications.DTOs;
using InternshipMatcher.Application.Features.Internships.Queries;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Applications.Commands;

public sealed record StudentApplyToInternshipOrchestrator(StudentApplyToInternshipDTO DTO) : IRequest<bool>;
public class StudentApplyToInternshipOrchestratorHandler : IRequestHandler<StudentApplyToInternshipOrchestrator, bool>
{
    private readonly IGeneralRepository<User> _repository;
    private readonly IGeneralRepository<ApplicationForm> _appRepo;
    private readonly IMediator _mediator;
    private readonly IAIService _aiService;
    public StudentApplyToInternshipOrchestratorHandler(IGeneralRepository<User> UserRepository , IGeneralRepository<ApplicationForm> appRepo, IMediator mediator , IAIService aiService)
    {
        this._repository = UserRepository;
        this._appRepo = appRepo;
        this._mediator = mediator;
        this._aiService = aiService;
    }
    public async Task<bool> Handle(StudentApplyToInternshipOrchestrator request, CancellationToken cancellationToken)
    {
        var userExists = await _repository.IsExistsAsync(request.DTO.StudentID);
        if (!userExists)
            return false;

        var internship = await _mediator.Send(new GetInternshipByIDQuery(request.DTO.InternshipID));
        if (internship is null)
            return false;

        var aiResult = await _aiService.GetMatchScoreAsync(request.DTO.Skills, request.DTO.Description);

        var application = new ApplicationForm
        {
            ID = Guid.NewGuid(),
            StudentProfileID = request.DTO.StudentProfileID,
            InternshipID = request.DTO.InternshipID,
            Description = request.DTO.Description,
            MatchScore = aiResult.Score
        };

        var added = await _appRepo.AddAsync(application);
        return added is not null;
    }
}

