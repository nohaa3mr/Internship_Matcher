using InternshipMatcher.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JWTService : IJWTService
{
    private readonly IConfiguration _config;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public JWTService(IConfiguration config)
    {
        _config = config;
    }

    public Task<string> GenerateToken(Guid userID, string email)
    {
        var key = GetSymmetricKey();
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

        return Task.FromResult(_tokenHandler.WriteToken(token));
    }

    public Task<string> GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Task.FromResult(Convert.ToBase64String(bytes));
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));

        return _tokenHandler.ValidateToken(token, BuildValidationParameters(validateLifetime: true), out _);
    }

    public async Task<ClaimsPrincipal?> ValidateExpiredToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        try
        {
            return await Task.Run(() =>
                _tokenHandler.ValidateToken(token, BuildValidationParameters(validateLifetime: false), out _)
            );
        }
        catch { return null; }
    }

    // ---

    private TokenValidationParameters BuildValidationParameters(bool validateLifetime) => new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = GetSymmetricKey(),

        ValidateIssuer = true,
        ValidIssuer = _config["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = _config["Jwt:Audience"],

        ValidateLifetime = validateLifetime,
        ClockSkew = TimeSpan.Zero
    };

    private SymmetricSecurityKey GetSymmetricKey() =>
        new(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
}