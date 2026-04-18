using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace InternshipMatcher.Infra.Services
{

    public class UserService : IUserService
    {
        private readonly IJWTService _jwtService;

        public UserService(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        public string GetUserIDFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            return  _jwtService.GetPrincipalFromToken(token)
                              .FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityTokenException("User ID claim not found in token.");
        }

        public async Task<bool> IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                _jwtService.GetPrincipalFromToken(token);
                return true;
            }
            catch { return false; }
        }
    }
}

