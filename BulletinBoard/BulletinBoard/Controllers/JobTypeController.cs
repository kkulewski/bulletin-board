using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobTypeViewModels;
using BulletinBoard.Services;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobTypeController : Controller
    {
        private readonly IJobTypeService _jobTypeService;

        public JobTypeController(IJobTypeService jobTypeService)
        {
            _jobTypeService = jobTypeService;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            var result = await _jobTypeService.GetAllTypes();
            var vm = Mapper.Map<IEnumerable<JobTypeViewModel>>(result);
            return View(vm);
        }

        // GET: JobType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            
            var jobType = await _jobTypeService.GetTypeById(id);
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

            var result = await _jobTypeService.Add(Mapper.Map<JobType>(model));
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }
        
        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _jobTypeService.GetTypeById(id);
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

            var type = Mapper.Map<JobType>(model);
            var result = await _jobTypeService.Edit(type);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }

        // GET: JobType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _jobTypeService.GetTypeById(id);
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

            var jobType = Mapper.Map<JobType>(model);
            var result = await _jobTypeService.Delete(jobType);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }
    }
}
