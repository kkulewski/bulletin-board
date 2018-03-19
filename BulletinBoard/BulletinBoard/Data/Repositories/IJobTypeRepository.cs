using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Models;

namespace BulletinBoard.Data.Repositories
{
    public interface IJobTypeRepository
    {
        Task<IEnumerable<JobType>> GetAll();

        Task<JobType> GetById(string id);

        Task Add(JobType item);

        Task Update(JobType item);

        Task Delete(JobType item);
    }
}
