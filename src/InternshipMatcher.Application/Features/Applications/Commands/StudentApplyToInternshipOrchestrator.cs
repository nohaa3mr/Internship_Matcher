using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Applications.DTOs;
using InternshipMatcher.Application.Features.Internships.Commands.UpdateInternship;
using InternshipMatcher.Application.Features.Internships.Queries;
using InternshipMatcher.Application.Features.Students.Queries;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Enums;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace InternshipMatcher.Application.Features.Applications.Commands;

public sealed record StudentApplyToInternshipOrchestrator(StudentApplyToInternshipRequestDTO DTO) : IRequest<Result<StudentApplyToInternshipResponseDTO>>;
public class StudentApplyToInternshipOrchestratorHandler : IRequestHandler<StudentApplyToInternshipOrchestrator, Result<StudentApplyToInternshipResponseDTO>>
{
    private readonly IGeneralRepository<ApplicationForm> _appRepo;
    private readonly IGeneralRepository<StudentApplication> _studentAppRepo;
    private readonly IGeneralRepository<Internship> _internshipRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAIService _ai;

    public StudentApplyToInternshipOrchestratorHandler(
    IGeneralRepository<Internship> internshipRepo,
    IGeneralRepository<ApplicationForm> appRepo,
    IGeneralRepository<StudentApplication> studentAppRepo,
    IUnitOfWork unitOfWork,
    IAIService aiService)
    {
        _internshipRepo = internshipRepo;
        _appRepo = appRepo;
        _studentAppRepo = studentAppRepo;
        _unitOfWork = unitOfWork;
        _ai = aiService;
    }
    public async Task<Result<StudentApplyToInternshipResponseDTO>> Handle(
      StudentApplyToInternshipOrchestrator request,
      CancellationToken cancellationToken)
    {
        var student = await _studentAppRepo.GetByIdAsync(request.DTO.StudentProfileID);
        if (student is null)
            return Result<StudentApplyToInternshipResponseDTO>.Failure("Student not found");

        var internship = await _internshipRepo.GetByIdAsync(request.DTO.InternshipID);
        if (internship is null)
            return Result<StudentApplyToInternshipResponseDTO>.Failure("Internship not found");

        var aiResult = await _ai.GetMatchScoreAsync(student.Skills, internship.Description);

        // 3. Application
        var application = new ApplicationForm
        {
            ID = Guid.NewGuid(),
            StudentProfileID = request.DTO.StudentProfileID,
            InternshipID = request.DTO.InternshipID,
            ApplicationStatus = ApplicationStatus.Pending,
            CoverLetter = request.DTO.CoverLetter,
            MatchScore = aiResult.Score
        };

        await _appRepo.AddAsync(application);

        // 4. StudentApplication
        var studentApp = new StudentApplication
        {
            ID = Guid.NewGuid(),
            StudentProfileID = request.DTO.StudentProfileID,
            InternshipID = request.DTO.InternshipID,
            ApplicationFormID = application.ID,
            AppliedAt = DateTime.UtcNow
        };

        await _studentAppRepo.AddAsync(studentApp);

        // 5. Update internship
        internship.ApplicantsCount += 1;
        await _internshipRepo.UpdateAsync(internship);

        // 6. SAVE ONCE
        await _unitOfWork.SaveChangesAsync();

        return Result<StudentApplyToInternshipResponseDTO>.Success(new StudentApplyToInternshipResponseDTO
        {
            ApplicationID = application.ID,
            MatchScore = aiResult.Score,
            Reasoning = aiResult.Reasoning
        });
    }
}

