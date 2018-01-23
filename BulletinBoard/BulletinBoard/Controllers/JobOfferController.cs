using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobOfferViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Controllers
{
    [Authorize]
    public class JobOfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public JobOfferController(ApplicationDbContext context, IMapper mapper,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Error(int? statusCode)
        {
            var vm = new ErrorViewModel
            {
                Response = statusCode?.ToString() ?? "-",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(vm);
        }

        // GET: JobOffer
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var jobOffers = await GetJobOffersGreedy()
                .Select(m => _mapper.Map<JobOfferViewModel>(m))
                .ToListAsync();

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                foreach (var offer in jobOffers)
                {
                    offer.CanEdit = offer.Author.Id == user.Id
                                    || await UserIsAdministrator()
                                    || await UserIsModerator();
                }
            }

            ViewData["JobOfferCount"] = jobOffers.Count;
            return View(jobOffers);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                return RedirectToAction(nameof(Index));
            }

            phrase = phrase.ToLower();

            // select each offer that contains search phrase in given fields, then map it to view models list
            var jobOffers = await GetJobOffersGreedy()
                .Where(c => c.Title.Contains(phrase)
                            || c.Description.ToLower().Contains(phrase)
                            || c.JobType.Name.ToLower().Contains(phrase)
                            || c.JobCategory.Name.ToLower().Contains(phrase)
                            || c.Author.Email.ToLower().Contains(phrase))
                .Select(m => _mapper.Map<JobOfferViewModel>(m))
                .ToListAsync();

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                foreach (var offer in jobOffers)
                {
                    offer.CanEdit = offer.Author.Id == user.Id
                                    || await UserIsAdministrator()
                                    || await UserIsModerator();
                }
            }

            // pass job offer count and search phrase to the view
            ViewData["JobOfferCount"] = jobOffers.Count;
            ViewData["phrase"] = phrase;

            return View("Index", jobOffers);
        }

        // GET: JobOffer/Popular
        [AllowAnonymous]
        public async Task<IActionResult> Popular()
        {
            // display top5 most popular offers
            var jobOffers = await GetJobOffersGreedy()
                .OrderByDescending(m => m.Visits)
                .Take(5)
                .Select(m => _mapper.Map<PopularJobOfferViewModel>(m))
                .ToListAsync();

            return View(jobOffers);
        }

        // GET: JobOffer/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobOffer = await GetJobOffersGreedy()
                .SingleOrDefaultAsync(m => m.JobOfferId == id);

            if (jobOffer == null)
            {
                return View("NotFound");
            }

            jobOffer.Visits += 1;
            _context.Update(jobOffer);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<DetailsJobOfferViewModel>(jobOffer);

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                viewModel.CanEdit = (viewModel.Author.Id == user.Id) || await UserIsAdministrator() || await UserIsModerator();
            }

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
                model.JobCategories = _context.JobCategories;
                model.JobTypes = _context.JobTypes;
                return View(model);
            }

            var jobOffer = new JobOffer
            {
                Author = await GetCurrentUser(),
                JobCategory = await _context.JobCategories.SingleOrDefaultAsync(c => c.JobCategoryId == model.JobCategoryId),
                JobType = await _context.JobTypes.SingleOrDefaultAsync(c => c.JobTypeId == model.JobTypeId),
                PostalCode = model.PostalCode,
                Title = model.Title,
                Description = model.Description,
                Submitted = DateTime.Now,
                LastEdit = DateTime.Now,
                Wage = model.Wage,
                Visits = 0
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
                return View("NotFound");
            }

            var jobOffer = await GetJobOffersGreedy()
                .SingleOrDefaultAsync(m => m.JobOfferId == id);

            if (jobOffer == null)
            {
                return View("NotFound");
            }

            if (jobOffer.Author.Id != (await GetCurrentUser()).Id && !await UserIsModerator() && !await UserIsAdministrator())
            {
                return View("AccessDenied");
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
                model.JobCategories = _context.JobCategories;
                model.JobTypes = _context.JobTypes;
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
                jobOffer.PostalCode = model.PostalCode;
                jobOffer.Title = model.Title;
                jobOffer.Description = model.Description;
                jobOffer.Wage = model.Wage;
                jobOffer.LastEdit = DateTime.Now;

                _context.Update(jobOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobOfferExists(model.JobOfferId))
                {
                    return View("NotFound");
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
                return View("NotFound");
            }

            var jobOffer = await _context.JobOffers
                .SingleOrDefaultAsync(m => m.JobOfferId == id);
            if (jobOffer == null)
            {
                return View("NotFound");
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

        private async Task<bool> JobOfferExists(string id)
        {
            return await _context.JobOffers.AnyAsync(e => e.JobOfferId == id);
        }

        private IQueryable<JobOffer> GetJobOffersGreedy()
        {
            // greedy load job offers (with all connected data)
            // as EntityFramework Core does not support lazy-loading yet
            return _context.JobOffers
                .Include(u => u.JobCategory)
                .Include(u => u.JobType)
                .Include(u => u.Author);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        private async Task<bool> UserIsModerator()
        {
            var user = await GetCurrentUser();
            return await _userManager.IsInRoleAsync(user, RoleHelper.Moderator);
        }

        private async Task<bool> UserIsAdministrator()
        {
            var user = await GetCurrentUser();
            return await _userManager.IsInRoleAsync(user, RoleHelper.Administrator);
        }
    }
}
