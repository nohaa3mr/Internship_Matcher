using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Features.Students.DTOs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Students.Queries
{
    public sealed record GetStudentByIDQuery(Guid StudentProfileID) : IRequest<Result<StudentProfileResponseDTO>>;
    public class GetStudentByIDQueryHandler: IRequestHandler<GetStudentByIDQuery, Result<StudentProfileResponseDTO>>
    {
        private readonly IGeneralRepository<StudentProfile> _repository;

        public GetStudentByIDQueryHandler(IGeneralRepository<StudentProfile> repository)
        {
            this._repository = repository;
        }

        public async Task<Result<StudentProfileResponseDTO>> Handle(GetStudentByIDQuery request, CancellationToken cancellationToken)
        {
            var studentProfile = await _repository.GetByIdAsync(request.StudentProfileID);
            if (studentProfile is null)
            {
                return Result<StudentProfileResponseDTO>.Failure("Student profile not found.");
            }
            return Result<StudentProfileResponseDTO>.Success(studentProfile.Adapt<StudentProfileResponseDTO>());
        }
    }


}
