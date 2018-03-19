using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly IJobCategoryRepository _repo;

        public JobCategoryService(IJobCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<JobCategory>> GetAllCategories()
        {
            return await _repo.GetAll();
        }

        public async Task<JobCategory> GetCategoryById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<bool> Add(JobCategory item)
        {
            await _repo.Add(item);
            return true;
        }

        public async Task<bool> Edit(JobCategory item)
        {
            var category = await _repo.GetById(item.JobCategoryId);
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

        public async Task<bool> Delete(JobCategory item)
        {
            await _repo.Delete(item);
            return true;
        }
    }
}
