namespace InternshipMatcher.Application.Features.Users.DTOs
{
    public class UserRegisterationResponseDTO
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string accessToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string refreshToken { get; set; }
        public bool IsRegistered { get; set; } = false;
    }
}