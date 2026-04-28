using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;

namespace InternshipMatcher.Application.Features.Students.Queries;

public sealed record IsStudentExistQuery(Guid StudentProfileID) : IRequest<bool>;
public class IsStudentExistQueryHandler : IRequestHandler<IsStudentExistQuery, bool>
{
    private readonly IGeneralRepository<StudentProfile> _repository;
    public IsStudentExistQueryHandler(IGeneralRepository<StudentProfile> repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(IsStudentExistQuery request, CancellationToken cancellationToken)
    {
        return  await _repository.IsExistsAsync(request.StudentProfileID);
    }
}
