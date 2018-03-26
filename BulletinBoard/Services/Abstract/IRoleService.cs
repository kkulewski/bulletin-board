using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulletinBoard.Services.Abstract
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRoles();

        Task<IdentityRole> GetUserRole(string userId);

        Task<bool> ChangeUserRole(string userId, string newRoleId);

        Task<bool> IsUserAdministrator(string userId);

        Task AddRole(string roleName);
    }
}
