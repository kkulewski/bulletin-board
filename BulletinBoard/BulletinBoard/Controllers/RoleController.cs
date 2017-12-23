using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    [Route("[controller]/[action]")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            var users = await _context.ApplicationUsers.ToListAsync();

            var viewModel = users.Select(user =>
            {
                var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                var userRoleId = _context.Roles.FirstOrDefault(role => role.NormalizedName == userRole).Id;
                var vm = new RoleViewModel
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    RoleId = userRoleId,
                    Roles = roles
                };
                return vm;
            });

            return View(viewModel);
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
            var user = _context.ApplicationUsers.First(c => c.Id == applicationUserId);
            var newRole = _context.Roles.First(c => c.Id == roleId);

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, newRole.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}