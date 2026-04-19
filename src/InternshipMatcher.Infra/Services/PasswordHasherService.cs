using InternshipMatcher.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Infra.Services
{
    using System.Security.Cryptography;

    public class PasswordHasherService : IPasswordHasher
    {
        private const int SaltSize = 32;        // 256-bit salt
        private const int HashSize = 32;        // 256-bit hash
        private const int Iterations = 600_000;   // OWASP 2023 recommendation
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public (string Hash, string Salt) Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                Iterations,
                Algorithm,
                HashSize
            );

            return (
                Hash: Convert.ToBase64String(hashBytes),
                Salt: Convert.ToBase64String(saltBytes)
            );
        }

        public bool Verify(string password, string hash, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("Salt cannot be null or empty.", nameof(salt));

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] expectedBytes = Convert.FromBase64String(hash);

            byte[] actualBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                Iterations,
                Algorithm,
                HashSize
            );

            return CryptographicOperations.FixedTimeEquals(actualBytes, expectedBytes);
        }
    }
}
