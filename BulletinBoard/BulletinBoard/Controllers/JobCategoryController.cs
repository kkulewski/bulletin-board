﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobCategoryViewModels;
using BulletinBoard.Services;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryService _jobCategoryService;

        public JobCategoryController(IJobCategoryService jobCategoryService)
        {
            _jobCategoryService = jobCategoryService;
        }

        // GET: JobCategory
        public async Task<IActionResult> Index()
        {
            var jobCategories = await _jobCategoryService.GetAllCategories();
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

            var category = await _jobCategoryService.GetCategoryById(id);
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

            var result = await _jobCategoryService.Add(Mapper.Map<JobCategory>(model));
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }

        // GET: JobCategory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var category = await _jobCategoryService.GetCategoryById(id);
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

            var category = Mapper.Map<JobCategory>(model);
            var result = await _jobCategoryService.Edit(category);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }

        // GET: JobCategory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var category = await _jobCategoryService.GetCategoryById(id);
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

            var category = Mapper.Map<JobCategory>(model);
            var result = await _jobCategoryService.Delete(category);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("NotFound");
        }
    }
}
