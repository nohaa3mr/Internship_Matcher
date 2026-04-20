using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Applications.DTOs;
using InternshipMatcher.Application.Features.Internships.Queries;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
namespace InternshipMatcher.Application.Features.Applications.Commands;

public sealed record StudentApplyToInternshipOrchestrator(StudentApplyToInternshipRequestDTO DTO) : IRequest<Result<StudentApplyToInternshipResponseDTO>>;
public class StudentApplyToInternshipOrchestratorHandler : IRequestHandler<StudentApplyToInternshipOrchestrator, Result<StudentApplyToInternshipResponseDTO>>
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
    public async Task<Result<StudentApplyToInternshipResponseDTO>> Handle(StudentApplyToInternshipOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Validate student profile exists
        var studentProfile = await _repository.GetByIdAsync(request.DTO.StudentProfileID);
        if (studentProfile is null)
            return Result<StudentApplyToInternshipResponseDTO>.Failure("Student profile not found");

        // 2. Validate internship exists
        var internship = await _mediator.Send(new GetInternshipByIDQuery(request.DTO.InternshipID));
        if (internship is null)
            return Result<StudentApplyToInternshipResponseDTO>.Failure("Internship not found");

        // 3. Run AI matching using DB data
        //var aiResult = await _aiService.GetMatchScoreAsync(
        //    studentProfile.Skills,
        //    internship.Description
        //);

        // 4. Create application
        var application = new ApplicationForm
        {
            ID = Guid.NewGuid(),
            StudentProfileID = request.DTO.StudentProfileID,
            InternshipID = request.DTO.InternshipID,
            CoverLetter = request.DTO.CoverLetter,
          //  MatchScore = aiResult.Score
        };

        var added = await _appRepo.AddAsync(application);
        var Result = added.Adapt<ApplicationForm>();
        return Result<StudentApplyToInternshipResponseDTO>.Success(Result.Adapt<StudentApplyToInternshipResponseDTO>());

    }
}

