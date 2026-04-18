using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IPasswordHasher
    {
     public (string Hash, string Salt) Hash(string password);
     public bool Verify(string password, string hash, string salt);
    }
}
