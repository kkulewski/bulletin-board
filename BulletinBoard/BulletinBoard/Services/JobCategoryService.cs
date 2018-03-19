using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly IJobCategoryRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public JobCategoryService(IJobCategoryRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
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
            _repo.Add(item);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Edit(JobCategory item)
        {
            var category = await _repo.GetById(item.JobCategoryId);
            category.Name = item.Name;

            try
            {
                _repo.Update(category);
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(JobCategory item)
        {
            _repo.Delete(item);
            await _unitOfWork.Save();
            return true;
        }
    }
}
