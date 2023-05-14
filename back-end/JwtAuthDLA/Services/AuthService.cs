using JwtAuthCore.Entities;
using JwtAuthCore.Services;
using JwtAuthCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthDAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> UserRegisterAsync(RegisterInputModel user)
        {
            var appUser = new ApplicationUser { UserName = user.Email, Email = user.Email };
            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("USER"))
                    await _roleManager.CreateAsync(new IdentityRole("USER"));

                if (await _roleManager.RoleExistsAsync("USER"))
                {
                    await _userManager.AddToRoleAsync(appUser, "USER");
                }
            }

            return result;
        }

        public async Task<AuthTokenResponse> UserLoginAsync(IdentityLoginRequestViewModal request)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(request.Email);
            if (appUser != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, request.Password, true, false);
                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(appUser);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, appUser.UserName),
                        new Claim(ClaimTypes.Email, appUser.Email),
                        new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                        new Claim("Id",appUser.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = GetToken(authClaims);
                    return new AuthTokenResponse() {
                        BearerToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo.ToLocalTime(),
                        Status = 200,
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        ProfileImg = appUser.ProfileImg,
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName,
                        Roles = userRoles,
                        Message = "Login successfully. Welcome to User Panel." 
                    };
                }
                return new AuthTokenResponse() { Status = StatusCodes.Status400BadRequest, Message = "User & Password Does not Match!" };
            }
            return new AuthTokenResponse() { Status = StatusCodes.Status400BadRequest, Message = "Email is not Verified or User is Not Registered!" };
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(28),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
