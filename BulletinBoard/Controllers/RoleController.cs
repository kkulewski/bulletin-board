using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers;
using BulletinBoard.Models.RoleViewModels;
using BulletinBoard.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IApplicationUserRepository _userRepo;

        public RoleController(IRoleService roleService, IApplicationUserRepository userRepo)
        {
            _roleService = roleService;
            _userRepo = userRepo;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAll();
            var roles = await _roleService.GetAllRoles();

            var viewModels = users.Select(user =>
                new RoleViewModel
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    RoleId = _roleService.GetUserRole(user.Id).Result.Id,
                    Roles = roles,
                    Disabled = _roleService.IsUserAdministrator(user.Id).Result
                }
            );

            return View(viewModels);
        }

        // POST: Role/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string applicationUserId, string newRoleId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleService.ChangeUserRole(applicationUserId, newRoleId);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }
    }
}