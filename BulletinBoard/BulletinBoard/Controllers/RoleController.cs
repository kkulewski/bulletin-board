using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Data.Repositories;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class RoleController : Controller
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(UserManager<ApplicationUser> userManager, IApplicationUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userManager = userManager;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAll();
            var roles = (await _roleRepo.GetAll()).ToList();

            var viewModels = users.Select(user =>
            {
                var userRoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                var userRole = roles.FirstOrDefault(x => x.NormalizedName == RoleHelper.Normalize(userRoleName));

                var vm = new RoleViewModel
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    RoleId = userRole?.Id,
                    Roles = roles,
                    Disabled = string.Equals(userRole?.NormalizedName, RoleHelper.Normalize(RoleHelper.Administrator))
                };
                return vm;
            });

            return View(viewModels);
        }

        // POST: Role/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string applicationUserId, string roleId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userRepo.GetById(applicationUserId);
            var newRole = await _roleRepo.GetById(roleId);

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, newRole.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}