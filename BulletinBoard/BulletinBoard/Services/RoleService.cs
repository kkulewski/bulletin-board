using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IApplicationUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(
            IRoleRepository roleRepo,
            IApplicationUserRepository userRepo,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoles()
        {
            return await _roleRepo.GetAll();
        }

        public async Task<IdentityRole> GetUserRole(string userId)
        {
            var user = await _userRepo.GetById(userId);

            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoleName = userRoleNames.FirstOrDefault();

            var roles = await _roleRepo.GetAll();
            return roles.FirstOrDefault(x => x.NormalizedName == RoleHelper.Normalize(userRoleName));
        }

        public async Task<bool> ChangeUserRole(string userId, string newRoleId)
        {
            var user = await _userRepo.GetById(userId);
            var newRole = await _roleRepo.GetById(newRoleId);
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, newRole.Name);

            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> IsUserAdministrator(string userId)
        {
            var userRole = await GetUserRole(userId);
            return string.Equals(userRole.NormalizedName, RoleHelper.Normalize(RoleHelper.Administrator));
        }
    }
}
