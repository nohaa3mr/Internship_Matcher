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
       Task< ClaimsPrincipal?> ValidateExpiredToken(string token);
    }
}
