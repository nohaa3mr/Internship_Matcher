using InternshipMatcher.Domain.Enums;
using System.Text.Json.Serialization;

namespace InternshipMatcher.Domain.Models;

public class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; } 
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    public bool RememberMe { get; set; } = true;
    public string RefreshToken { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;
    public string Skills { get; set; }
}