using InternshipMatcher.Domain.Enums;

namespace InternshipMatcher.Domain.Models;

public class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    public bool RememberMe { get; set; }
    public string RefreshToken { get; set; }
    public  UserRole Role { get; set; } 
}