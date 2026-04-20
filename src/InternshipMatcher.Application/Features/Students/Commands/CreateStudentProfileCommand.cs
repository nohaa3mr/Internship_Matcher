using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Students.DTOs;
using MediatR;

namespace InternshipMatcher.Application.Features.Students.Commands
{
   public sealed record class CreateStudentProfileCommand(CreateStudentProfileRequestDTO DTO) : IRequest<Result<StudentProfileResponseDTO>>;
    
   
}
