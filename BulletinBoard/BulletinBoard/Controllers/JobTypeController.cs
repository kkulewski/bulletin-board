using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobTypeViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobTypeController : Controller
    {
        private readonly IJobTypeRepository _jobTypeRepo;

        public JobTypeController(IJobTypeRepository jobTypeRepo)
        {
            _jobTypeRepo = jobTypeRepo;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            var types = await _jobTypeRepo.GetAll();
            var vm = Mapper.Map<IEnumerable<JobTypeViewModel>>(types);
            return View(vm);
        }

        // GET: JobType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _jobTypeRepo.GetById(id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var vm = Mapper.Map<DetailsJobTypeViewModel>(jobType);
            return View(vm);
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

            await _jobTypeRepo.Add(new JobType { Name = model.Name });
            return RedirectToAction(nameof(Index));
        }
        
        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _jobTypeRepo.GetById(id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var vm = Mapper.Map<EditJobTypeViewModel>(jobType);
            return View(vm);
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
                var jobType = await _jobTypeRepo.GetById(model.JobTypeId);
                jobType.Name = model.Name;
                await _jobTypeRepo.Update(jobType);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobTypeExists(model.JobTypeId))
                {
                    return View("NotFound");
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
                return View("NotFound");
            }

            var jobType = await _jobTypeRepo.GetById(id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var vm = Mapper.Map<DeleteJobTypeViewModel>(jobType);
            return View(vm);
        }

        // POST: JobType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobType = await _jobTypeRepo.GetById(model.JobTypeId);
            await _jobTypeRepo.Delete(jobType);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobTypeExists(string id)
        {
            return await _jobTypeRepo.GetById(id) != null;
        }
    }
}
