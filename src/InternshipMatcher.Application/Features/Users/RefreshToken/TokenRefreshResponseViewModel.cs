using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Features.Users.RefreshToken
{
    public class TokenRefreshResponseViewModel
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

    }
}
