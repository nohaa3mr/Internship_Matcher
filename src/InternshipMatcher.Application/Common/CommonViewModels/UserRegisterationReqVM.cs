namespace InternshipMatcher.API.Common.CommonViewModels
{
    public class UserRegisterationReqVM
    {
        public Guid ID { get;  set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get;  set; }
    }
}
