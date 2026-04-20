using InternshipMatcher.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Infra.Services
{
    using System.Security.Cryptography;

    public class PasswordHasherService : IPasswordHasher
    {
        private const int SaltSize = 32;        
        private const int HashSize = 32;        
        private const int Iterations = 600_000;  
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public string  Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); 
            byte[] Hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                Algorithm,
                HashSize
            );
            return $"{Convert.ToBase64String(Hash)}.{Convert.ToBase64String(salt)}";
        }

        public bool Verify(string RequestPassword, string HashPassword)
        {
            var parts = HashPassword.Split('.', 2);
            if (parts.Length != 2)
                return false;
            byte[] salt = Convert.FromHexString(parts[1]);
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                RequestPassword,
                salt,
                Iterations,
                Algorithm,
                HashSize
            );
            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
