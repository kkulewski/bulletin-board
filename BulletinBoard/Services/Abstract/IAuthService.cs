using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BulletinBoard.Helpers;
using BulletinBoard.Models;

namespace BulletinBoard.Services.Abstract
{
    public interface IAuthService
    {
        Task<bool> Register(string userName, string password, bool signIn = true, string roleName = RoleHelper.User);

        Task<bool> Logout();

        Task<bool> Login(string userName, string password, bool rememberMe);

        Task<bool> IsSignedIn(ClaimsPrincipal claimsUser);

        Task<ApplicationUser> GetSignedUser(ClaimsPrincipal claimsUser);

        Task<bool> IsInRole(ApplicationUser user, string roleName);

        Task<IList<string>> GetUserRoles(ApplicationUser user);

        Task<bool> AddRoleToUser(ApplicationUser user, string roleName);

        Task<bool> RemoveRolesFromUser(ApplicationUser user, IList<string> roleNames);

        Task<string> GetUserName(ClaimsPrincipal claimsUser);
    }
}
