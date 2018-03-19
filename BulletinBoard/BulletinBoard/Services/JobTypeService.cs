using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly IJobTypeRepository _repo;

        public JobTypeService(IJobTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobType>> GetAllTypes()
        {
            return await _repo.GetAll();
        }

        public async Task<JobType> GetTypeById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<bool> Add(JobType item)
        {
            await _repo.Add(item);
            return true;
        }

        public async Task<bool> Edit(JobType item)
        {
            var category = await _repo.GetById(item.JobTypeId);
            category.Name = item.Name;

            try
            {
                await _repo.Update(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(JobType item)
        {
            await _repo.Delete(item);
            return true;
        }
    }
}
