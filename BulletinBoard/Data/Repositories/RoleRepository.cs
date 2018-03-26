using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdentityRole>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetById(string id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Add(IdentityRole role)
        {
            _context.Roles.Add(role);
        }
    }
}
