using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using MediatR;

namespace InternshipMatcher.Application.Features.Users.RefreshToken;

public sealed record  RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenResponseDTO>>;
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponseDTO>>
{
    private readonly IGeneralRepository<User> _repository;
    private readonly IPasswordHasher _hasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;

    public RefreshTokenCommandHandler(IGeneralRepository<User> repository , IPasswordHasher hasher , IUnitOfWork unitOfWork , IJWTService jwtService)
    {
        _unitOfWork = unitOfWork;
        this._jwtService = jwtService;
        _repository = repository;
        this._hasher = hasher;
    }



    public async Task<Result<RefreshTokenResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {

        var hashed = _hasher.Hash(request.RefreshToken);
        var user = await _repository.GetByPredicateAsync(
            u => u.RefreshToken == hashed);
        if (user == null)
            return Result<RefreshTokenResponseDTO>.Failure("Invalid refresh token");

        if (user.IsDeleted || !user.IsActive)
            return Result<RefreshTokenResponseDTO>.Failure("User is not active");

        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return Result<RefreshTokenResponseDTO>.Failure("Refresh token has expired");

        var newRefreshToken = await _jwtService.GenerateRefreshToken();
        var accessToken = await _jwtService.GenerateToken(user.ID, user.Email);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _repository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result<RefreshTokenResponseDTO>.Success(new RefreshTokenResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        });
    }
}



