using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobTypeViewModels;
using BulletinBoard.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobTypeController : Controller
    {
        private readonly IJobTypeService _jobTypeService;
        private readonly IMapper _mapper;

        public JobTypeController(IJobTypeService jobTypeService, IMapper mapper)
        {
            _jobTypeService = jobTypeService;
            _mapper = mapper;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            var result = await _jobTypeService.GetAllTypes();
            var vm = _mapper.Map<IEnumerable<JobTypeViewModel>>(result);
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

            var vm = _mapper.Map<DetailsJobTypeViewModel>(jobType);
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

            var vm = _mapper.Map<EditJobTypeViewModel>(jobType);
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

            var type = _mapper.Map<JobType>(model);
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

            var vm = _mapper.Map<DeleteJobTypeViewModel>(jobType);
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

            var jobType = _mapper.Map<JobType>(model);
            var result = await _jobTypeService.Delete(jobType);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }
    }
}
