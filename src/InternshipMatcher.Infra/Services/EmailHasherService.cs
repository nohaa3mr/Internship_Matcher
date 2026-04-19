using InternshipMatcher.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Infra.Services
{
    using BCrypt.Net;

    public class EmailHasherService : IEmailHasher
    {
        private const int WorkFactor = 12;

        public string Hash(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            string normalizedEmail = email.Trim().ToLowerInvariant();
            return BCrypt.HashPassword(normalizedEmail, WorkFactor);
        }

        public bool Verify(string email, string hash)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

            string normalizedEmail = email.Trim().ToLowerInvariant();
            return BCrypt.Verify(normalizedEmail, hash);
        }
    }
}
