using JwtAuthCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthInfrastructure.Data
{
	public static class DbInitializer
	{
		public static async Task InitializeAsync(IServiceProvider serviceProvider, UserManager<ApplicationUser> _userManager)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			string[] roleNames = { "SuperAdmin", "Admin", "User" };
			IdentityResult result;
			foreach (var roleName in roleNames)
			{
				var roleExists = await roleManager.RoleExistsAsync(roleName);
				if (!roleExists)
				{
					result = await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
			string Email = "superadmin@gmail.com";
			string Password = "Superadmin@123";
			if (_userManager.FindByEmailAsync(Email).Result == null)
			{
				ApplicationUser user = new ApplicationUser();
				user.UserName = Email;
				user.Email = Email;
				IdentityResult identityResult = _userManager.CreateAsync(user, Password).Result;
				if (identityResult.Succeeded)
				{
					_userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
				}
			}
		}
	}
}
