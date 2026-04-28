using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Students.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;

namespace InternshipMatcher.Application.Features.Students.Commands;

public sealed record class CreateStudentProfileCommand(CreateStudentProfileRequestDTO DTO) : IRequest<Result<StudentProfileResponseDTO>>;
public class CreateStudentProfileCommandHandler : IRequestHandler<CreateStudentProfileCommand, Result<StudentProfileResponseDTO>>
{
    private readonly IGeneralRepository<StudentProfile> _studentRepo;

    public CreateStudentProfileCommandHandler(IGeneralRepository<StudentProfile> studentRepo)
    {
        _studentRepo = studentRepo;
    }
    public async Task<Result<StudentProfileResponseDTO>> Handle(CreateStudentProfileCommand request, CancellationToken cancellationToken)
    {
        var studentProfile = request.DTO.Adapt<StudentProfile>();
      await  _studentRepo.AddAsync(studentProfile);
        var responseDTO = new StudentProfileResponseDTO{};
        return Result<StudentProfileResponseDTO>.Success(responseDTO, "Student profile created successfully.");
    }
}
