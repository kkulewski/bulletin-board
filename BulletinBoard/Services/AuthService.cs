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

        public async Task<bool> Register(string userName, string password, bool signIn = true, string roleName = RoleHelper.User)
        {
            var createdUser = await CreateUser(userName, password);
            if (createdUser == null)
            {
                return false;
            }

            var roleAssigned = await AddRoleToUser(createdUser, roleName);
            if (!roleAssigned)
            {
                return false;
            }

            if (signIn)
            {
                await _signInManager.SignInAsync(createdUser, isPersistent: false);
            }

            // TODO: return status codes for further error handling
            return true;
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
            if (claimsUser == null)
            {
                return false;
            }

            return await Task.Run(() => _signInManager.IsSignedIn(claimsUser));
        }

        public async Task<ApplicationUser> GetSignedUser(ClaimsPrincipal claimsUser)
        {
            if (claimsUser == null)
            {
                return null;
            }

            return await _userManager.GetUserAsync(claimsUser);
        }

        public async Task<bool> IsInRole(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRolesFromUser(ApplicationUser user, IList<string> roleNames)
        {
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<string> GetUserName(ClaimsPrincipal claimsUser)
        {
            if (claimsUser == null)
            {
                return null;
            }

            return await Task.Run(() => _userManager.GetUserName(claimsUser));
        }

        private async Task<ApplicationUser> CreateUser(string userName, string password)
        {
            var user = new ApplicationUser { UserName = userName, Email = userName };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                user = null;
            }

            return user;
        }
    }
}
