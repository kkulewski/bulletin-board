using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Controllers
{
    [Produces("application/json")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: role/
        [HttpGet]
        public IEnumerable<string> Index()
        {
            if (RolesEmpty())
            {
                var roles = new List<string>
                {
                    RoleHelper.Administrator,
                    RoleHelper.Moderator,
                    RoleHelper.User
                };

                roles.ForEach(AddRole);
                _context.SaveChangesAsync();
            }

            return _context.Roles.Select(c => c.Name);
        }

        [HttpGet]
        [Authorize(Roles = RoleHelper.Administrator)]
        public string AdministratorTest()
        {
            return "administrator";
        }

        [HttpGet]
        [Authorize(Roles = RoleHelper.Moderator)]
        public string ModeratorTest()
        {
            return "moderator";
        }

        [HttpGet]
        [Authorize(Roles = RoleHelper.User)]
        public string UserTest()
        {
            return "user";
        }

        private void AddRole(string roleName)
        {
            var role = new IdentityRole(roleName) { NormalizedName = RoleHelper.Normalize(roleName) };
            _context.Roles.Add(role);
        }

        private bool RolesEmpty()
        {
            return !_context.Roles.Any();
        }
    }
}