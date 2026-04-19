using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Interfaces;

public interface IUserService
{
    string  GetUserIDFromToken(string token);
    Task<bool> IsTokenValid(string token);
    
}
