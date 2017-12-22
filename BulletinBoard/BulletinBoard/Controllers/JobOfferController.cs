using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Models;
using BulletinBoard.Models.JobOfferViewModels;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobOfferController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: JobOffer
        public async Task<IActionResult> Index()
        {
            var jobOffers = await GetJobOffersGreedy()
                .Select(m => _mapper.Map<JobOfferViewModel>(m))
                .ToListAsync();

            return View(jobOffers);
        }

        // GET: JobOffer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await GetJobOffersGreedy()
                .SingleOrDefaultAsync(m => m.JobOfferId == id);

            if (jobOffer == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DetailsJobOfferViewModel>(jobOffer);
            return View(viewModel);
        }

        // GET: JobOffer/Create
        public IActionResult Create()
        {
            var viewModel = new CreateJobOfferViewModel
            {
                JobCategories = _context.JobCategories,
                JobTypes = _context.JobTypes
            };

            return View(viewModel);
        }

        // POST: JobOffer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobOffer = new JobOffer
            {
                Author = await _userManager.GetUserAsync(HttpContext.User),
                JobCategory = await _context.JobCategories.SingleOrDefaultAsync(c => c.JobCategoryId == model.JobCategoryId),
                JobType = await _context.JobTypes.SingleOrDefaultAsync(c => c.JobTypeId == model.JobTypeId),
                Title = model.Title,
                Description = model.Description,
                Submitted = DateTime.Now,
                LastEdit = DateTime.Now,
                Wage = model.Wage,
                Active = true

            };

            _context.Add(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobOffer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await GetJobOffersGreedy()
                .SingleOrDefaultAsync(m => m.JobOfferId == id);

            if (jobOffer == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditJobOfferViewModel>(jobOffer);
            viewModel.JobCategories = _context.JobCategories;
            viewModel.JobTypes = _context.JobTypes;
            return View(viewModel);
        }

        // POST: JobOffer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var jobOffer = await _context.JobOffers
                    .SingleOrDefaultAsync(m => m.JobOfferId == model.JobOfferId);

                jobOffer.JobCategory = await _context.JobCategories
                    .SingleOrDefaultAsync(c => c.JobCategoryId == model.JobCategoryId);
                jobOffer.JobType = await _context.JobTypes
                    .SingleOrDefaultAsync(c => c.JobTypeId == model.JobTypeId);
                jobOffer.Title = model.Title;
                jobOffer.Description = model.Description;
                jobOffer.Wage = model.Wage;
                jobOffer.LastEdit = DateTime.Now;
                jobOffer.Active = model.Active;

                _context.Update(jobOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobOfferExists(model.JobOfferId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobOffer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .SingleOrDefaultAsync(m => m.JobOfferId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DeleteJobOfferViewModel>(jobOffer);
            return View(viewModel);
        }

        // POST: JobOffer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteJobOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobOffer = await _context.JobOffers
                .SingleOrDefaultAsync(m => m.JobOfferId == model.JobOfferId);
            _context.JobOffers.Remove(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOfferExists(string id)
        {
            return _context.JobOffers.Any(e => e.JobOfferId == id);
        }

        private IQueryable<JobOffer> GetJobOffersGreedy()
        {
            return _context.JobOffers
                .Include(u => u.JobCategory)
                .Include(u => u.JobType)
                .Include(u => u.Author)
                .Where(t => t.Active);
        }
    }
}
