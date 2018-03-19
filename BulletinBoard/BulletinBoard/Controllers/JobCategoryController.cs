using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobCategoryViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryRepository _jobCategoryRepo;

        public JobCategoryController(IJobCategoryRepository jobCategoryRepo)
        {
            _jobCategoryRepo = jobCategoryRepo;
        }

        // GET: JobCategory
        public async Task<IActionResult> Index()
        {
            var jobCategories = await _jobCategoryRepo.GetAll();
            var vm = Mapper.Map<IEnumerable<JobCategoryViewModel>>(jobCategories);
            return View(vm);
        }

        // GET: JobCategory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            var category = await _jobCategoryRepo.GetById(id);
            if (category == null)
            {
                return View("NotFound");
            }
            
            var vm = Mapper.Map<DetailsJobCategoryViewModel>(category);
            return View(vm);
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

            await _jobCategoryRepo.Add(new JobCategory { Name = model.Name });
            return RedirectToAction(nameof(Index));
        }

        // GET: JobCategory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var category = await _jobCategoryRepo.GetById(id);
            if (category == null)
            {
                return View("NotFound");
            }

            var vm = Mapper.Map<EditJobCategoryViewModel>(category);
            return View(vm);
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
                var category = await _jobCategoryRepo.GetById(model.JobCategoryId);
                category.Name = model.Name;
                await _jobCategoryRepo.Update(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobCategoryExists(model.JobCategoryId))
                {
                    return View("NotFound");
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
                return View("NotFound");
            }

            var category = await _jobCategoryRepo.GetById(id);
            if (category == null)
            {
                return View("NotFound");
            }

            var vm = Mapper.Map<DeleteJobCategoryViewModel>(category);
            return View(vm);
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

            var category = await _jobCategoryRepo.GetById(model.JobCategoryId);
            await _jobCategoryRepo.Delete(category);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobCategoryExists(string id)
        {
            return await _jobCategoryRepo.GetById(id) != null;
        }
    }
}
