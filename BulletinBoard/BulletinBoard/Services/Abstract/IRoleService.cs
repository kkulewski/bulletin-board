using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulletinBoard.Services.Abstract
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<IdentityRole> GetUserRole(string userID);

        Task<bool> ChangeUserRole(string userId, string newRoleId);

        Task<bool> IsUserAdministrator(string userId);
    }
}
