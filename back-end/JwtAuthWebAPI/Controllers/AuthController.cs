using JwtAuthCore.Entities;
using JwtAuthCore.Services;
using JwtAuthCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JwtAuthWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel user)
        {
            IdentityResult result = await _authService.UserRegisterAsync(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(IdentityLoginRequestViewModal request)
        {
            AuthTokenResponse result = await _authService.UserLoginAsync(request);
            return Ok(result);
        }
    }
}
