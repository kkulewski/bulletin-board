using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobTypeViewModels;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Controllers
{
    [Route("[controller]/[action]")]
    public class JobTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            var jobTypes = await _context.JobTypes.ToListAsync();
            return View(jobTypes);
        }

        // GET: JobType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // GET: JobType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobType = new JobType {Name = model.Name};
            _context.Add(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobTypes.SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound();
            }

            var viewModel = new EditJobTypeViewModel
            {
                JobTypeId = jobType.JobTypeId,
                Name = jobType.Name
            };

            return View(viewModel);
        }

        // POST: JobType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var jobType = await _context.JobTypes.SingleOrDefaultAsync(m => m.JobTypeId == model.JobTypeId);
                jobType.Name = model.Name;
                _context.Update(jobType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTypeExists(model.JobTypeId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // POST: JobType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string jobTypeId)
        {
            var jobType = await _context.JobTypes.SingleOrDefaultAsync(m => m.JobTypeId == jobTypeId);
            _context.JobTypes.Remove(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobTypeExists(string id)
        {
            return _context.JobTypes.Any(e => e.JobTypeId == id);
        }
    }
}
