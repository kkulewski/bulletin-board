using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;

namespace BulletinBoard.Services
{
    public class ApplicationApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _userRepository;

        public ApplicationApplicationUserService(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }
    }
}
