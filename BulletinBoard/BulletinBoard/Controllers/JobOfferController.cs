using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Models;
using BulletinBoard.Models.JobOfferViewModels;

namespace BulletinBoard.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobOfferController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: JobOffer
        public async Task<IActionResult> Index()
        {
            var jobOffers = await _context.JobOffers
                .Where(t => t.Active)
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

            var jobOffer = await _context.JobOffers
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
            return View();
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
                //TODO: Author = USER,
                Title = model.Title,
                Description = model.Description,
                Submitted = DateTime.Now,
                LastEdit = DateTime.Now,
                Wage = model.Wage,
                Active = true,
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

            var jobOffer = await _context.JobOffers
                .SingleOrDefaultAsync(m => m.JobOfferId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditJobOfferViewModel>(jobOffer);
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
    }
}
