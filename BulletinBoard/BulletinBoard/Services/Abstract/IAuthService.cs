using System.Security.Claims;
using System.Threading.Tasks;
using BulletinBoard.Models;

namespace BulletinBoard.Services.Abstract
{
    public interface IAuthService
    {
        Task<bool> Register(string userName, string password);

        Task<bool> Logout();

        Task<bool> Login(string userName, string password, bool rememberMe);

        Task<bool> IsSignedIn(ClaimsPrincipal claimsUser);

        Task<bool> IsInRole(ApplicationUser user, string roleName);

        Task<ApplicationUser> GetSignedUser(ClaimsPrincipal claimsUser);
    }
}
