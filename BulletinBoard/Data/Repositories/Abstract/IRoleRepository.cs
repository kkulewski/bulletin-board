using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Data.Repositories.Abstract
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAll();

        Task<IdentityRole> GetById(string id);

        void Add(IdentityRole role);
    }
}
