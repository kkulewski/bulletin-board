using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Models;

namespace BulletinBoard.Data.Repositories
{
    public interface IJobCategoryRepository
    {
        Task<IEnumerable<JobCategory>> GetAll();

        Task<JobCategory> GetById(string id);

        Task Add(JobCategory item);

        Task Update(JobCategory item);

        Task Delete(JobCategory item);
    }
}
