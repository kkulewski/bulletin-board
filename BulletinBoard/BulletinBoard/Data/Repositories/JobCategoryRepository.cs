using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Data.Repositories
{
    internal class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public JobCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobCategory>> GetAll()
        {
            return await _context.JobCategories.ToListAsync();
        }

        public async Task<JobCategory> GetById(string id)
        {
            return await _context.JobCategories.FirstOrDefaultAsync(x => x.JobCategoryId == id);
        }

        public void Add(JobCategory item)
        {
            _context.Add(item);
        }

        public void Update(JobCategory item)
        {
            _context.Update(item);
        }

        public void Delete(JobCategory item)
        {
            _context.Remove(item);
        }
    }
}
