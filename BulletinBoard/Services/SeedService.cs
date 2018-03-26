using System.Linq;
using System.Threading.Tasks;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using BulletinBoard.Services.Abstract;

namespace BulletinBoard.Services
{
    public class SeedService : ISeedService
    {
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SeedService(
            IAuthService authService,
            IRoleService roleService,
            IApplicationUserService applicationUserService,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _roleService = roleService;
            _applicationUserService = applicationUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Seed()
        {
            var roles = await _roleService.GetAllRoles();
            if (!roles.Any())
            {
                var roleNames = new[]
                {
                    RoleHelper.Administrator,
                    RoleHelper.Moderator,
                    RoleHelper.User
                };

                foreach (var roleName in roleNames)
                {
                    await _roleService.AddRole(roleName);
                }
            }

            var users = await _applicationUserService.GetAllUsers();
            if (!users.Any())
            {
                await _authService.Register("admin@admin.com", "admin", signIn: false, roleName: RoleHelper.Administrator);
            }

            await _unitOfWork.Save();
        }
    }
}
