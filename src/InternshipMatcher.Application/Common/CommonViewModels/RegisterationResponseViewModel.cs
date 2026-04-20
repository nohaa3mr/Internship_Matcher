using InternshipMatcher.Domain.Enums;

namespace InternshipMatcher.API.Common.CommonViewModels
{
    public class RegisterationResponseViewModel
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
