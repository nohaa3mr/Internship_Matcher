using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IJWTService
    {
        string GenerateAccessToken(Guid userID, string email, string roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateExpiredToken(string token);
    }
}
