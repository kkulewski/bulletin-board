using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Data
{
    public class MigrationManager : IMigrationManager
    {
        private readonly ApplicationDbContext _context;

        public MigrationManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Apply()
        {
            await _context.Database.MigrateAsync();
        }
    }
}
