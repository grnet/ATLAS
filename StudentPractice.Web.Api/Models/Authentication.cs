using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.Web.Api
{

    public class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationResult
    {
        public string AuthToken { get; set; }
    }
}
