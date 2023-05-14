using JwtAuthCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthCore.Services
{
    public interface IAuthService
    {
        public Task<IdentityResult> UserRegisterAsync(RegisterInputModel model);
        public Task<AuthTokenResponse> UserLoginAsync(IdentityLoginRequestViewModal model);
    }
}
