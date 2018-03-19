using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Models;

namespace BulletinBoard.Data.Repositories.Abstract
{
    public interface IJobTypeRepository
    {
        Task<IEnumerable<JobType>> GetAll();

        Task<JobType> GetById(string id);

        void Add(JobType item);

        void Update(JobType item);

        void Delete(JobType item);
    }
}
