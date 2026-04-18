using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IEmailHasher
    {
        string Hash(string email);
        bool Verify(string email, string hash);
    }
}
