using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateToken(Guid userID, string email);
        Task<string> GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);       // for valid tokens
        Task<ClaimsPrincipal?> ValidateExpiredToken(string token); // for refresh flow
    }
}
