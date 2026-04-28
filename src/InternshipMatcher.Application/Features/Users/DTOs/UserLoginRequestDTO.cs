namespace InternshipMatcher.Application.Features.Users.DTOs
{
    public class UserLoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid ID { get; internal set; }
    }
}