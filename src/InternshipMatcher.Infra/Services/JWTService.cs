using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InternshipMatcher.Infra.Services
{ 
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;


        public JWTService(IConfiguration config)
        { 
            this._config = config;
        }

        public async Task<string> GenerateToken(Guid userID, string email)
        {
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userID.ToString()),
            new Claim(ClaimTypes.Email, email),
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return  await Task.FromResult(Convert.ToBase64String(bytes));
        }

        public async Task<ClaimsPrincipal?> ValidateExpiredToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateLifetime = false  // allow expired — we're refreshing
            };

            try { return await Task.Run(() => handler.ValidateToken(token, parameters, out _)); }
            catch { return null; }
        }
    }    
}
