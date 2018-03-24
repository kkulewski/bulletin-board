using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Register(string userName, string password)
        {
            var user = new ApplicationUser { UserName = userName, Email = userName };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleHelper.User);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }

            return false;
        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<bool> Login(string userName, string password, bool rememberMe)
        {
            var result = await _signInManager
                .PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> IsSignedIn(ClaimsPrincipal claimsUser)
        {
            return await Task.Run(() => _signInManager.IsSignedIn(claimsUser));
        }

        public async Task<ApplicationUser> GetSignedUser(ClaimsPrincipal claimsUser)
        {
            return await _userManager.GetUserAsync(claimsUser);
        }

        public async Task<bool> IsInRole(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRolesFromUser(ApplicationUser user, IList<string> roleNames)
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            return result.Succeeded;
        }
    }
}
