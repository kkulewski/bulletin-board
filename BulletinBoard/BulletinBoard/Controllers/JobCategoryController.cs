using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Models;
using BulletinBoard.Models.JobCategoryViewModels;

namespace BulletinBoard.Controllers
{
    [Route("[controller]/[action]")]
    public class JobCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobCategoryController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: JobCategory
        public async Task<IActionResult> Index()
        {
            var jobCategories = await _context.JobCategories
                .Select(m => _mapper.Map<JobCategoryViewModel>(m)).ToListAsync();
            return View(jobCategories);
        }

        // GET: JobCategory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategories
                .SingleOrDefaultAsync(m => m.JobCategoryId == id);
            if (jobCategory == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DetailsJobCategoryViewModel>(jobCategory);
            return View(viewModel);
        }

        // GET: JobCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobCategory = new JobCategory {Name = model.Name};
            _context.Add(jobCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobCategory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategories
                .SingleOrDefaultAsync(m => m.JobCategoryId == id);
            if (jobCategory == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditJobCategoryViewModel>(jobCategory);
            return View(viewModel);
        }

        // POST: JobCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var jobCategory = await _context.JobCategories
                    .SingleOrDefaultAsync(m => m.JobCategoryId == model.JobCategoryId);
                jobCategory.Name = model.Name;
                _context.Update(jobCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobCategoryExists(model.JobCategoryId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobCategory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategories
                .SingleOrDefaultAsync(m => m.JobCategoryId == id);
            if (jobCategory == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DeleteJobCategoryViewModel>(jobCategory);
            return View(viewModel);
        }

        // POST: JobType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteJobCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobCategory = await _context.JobCategories
                .SingleOrDefaultAsync(m => m.JobCategoryId == model.JobCategoryId);
            _context.JobCategories.Remove(jobCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobCategoryExists(string id)
        {
            return _context.JobCategories.Any(e => e.JobCategoryId == id);
        }
    }
}
