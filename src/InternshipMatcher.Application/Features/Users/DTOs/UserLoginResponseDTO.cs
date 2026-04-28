namespace InternshipMatcher.Application.Features.Users.DTOs
{
    public class UserLoginResponseDTO
    {
        public string Token { get;  set; }
        public string Email { get;  set; }
        public string Password { get;  set; }
        public string RefreshToken { get;  set; }
        public string AccessToken { get;  set; }
        public Guid UserID { get;  set; }
        public string Role { get;  set; }
        public Guid ID { get; internal set; }
        public string UserRole { get; internal set; }
        public string UserName { get; internal set; }
    }
}