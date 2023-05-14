using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthCore.ViewModels
{
    public class AuthTokenResponse
    {
        public string BearerToken { get; set; }
        public DateTime Expiration { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImg { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Message { get; set; }
        public IList<string> Roles { get; set; }
    }
}
