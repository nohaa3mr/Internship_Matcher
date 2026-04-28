using InternshipMatcher.Domain.Enums;
using System.Text.Json.Serialization;

namespace InternshipMatcher.Domain.Models;

public class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string Password { get; set; }
    public bool RememberMe { get; set; } = true;
    public string RefreshToken { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}