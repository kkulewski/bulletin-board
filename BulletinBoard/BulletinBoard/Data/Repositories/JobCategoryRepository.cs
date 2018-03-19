﻿using System.Collections.Generic;
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

        public async Task Add(JobCategory item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(JobCategory item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(JobCategory item)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
