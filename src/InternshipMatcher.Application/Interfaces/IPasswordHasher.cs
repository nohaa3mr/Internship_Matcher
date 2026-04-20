namespace InternshipMatcher.Application.Interfaces;

public interface IPasswordHasher
{
 public string Hash(string password);
 public bool Verify(string InputPassword, string hashPassword);
}
