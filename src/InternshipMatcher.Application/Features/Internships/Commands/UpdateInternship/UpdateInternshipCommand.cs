using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Internships.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;
namespace InternshipMatcher.Application.Features.Internships.Commands.UpdateInternship;

public sealed record UpdateInternshipCommand(Guid InternshipID) : IRequest<Result<UpdateInternshipResponseDTO>>;
public class UpdateInternshipCommandHandler : IRequestHandler<UpdateInternshipCommand, Result<UpdateInternshipResponseDTO>>
{
    private readonly IGeneralRepository<Internship> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInternshipCommandHandler(IGeneralRepository<Internship> repository , IUnitOfWork unitOfWork)
    {
        this._repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<UpdateInternshipResponseDTO>> Handle(UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        if (request.InternshipID == Guid.Empty)
            return  Result<UpdateInternshipResponseDTO>.Failure("ID can not be empty.");
        var intern = await _repository.GetByIdAsync(request.InternshipID);
        var Updated = _repository.UpdateAsync(intern);
        if (Updated != null)
            return Result<UpdateInternshipResponseDTO>.Success();
       await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
