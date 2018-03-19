using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Services
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IJobOfferRepository _jobOfferRepo;
        private readonly IJobCategoryRepository _jobCategoryRepo;
        private readonly IJobTypeRepository _jobTypeRepo;
        private readonly IApplicationUserRepository _applicationUserRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public JobOfferService(
            IJobOfferRepository jobOfferRepo,
            IJobCategoryRepository jobCategoryRepo,
            IJobTypeRepository jobTypeRepo,
            IApplicationUserRepository applicationUserRepo,
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _jobOfferRepo = jobOfferRepo;
            _jobCategoryRepo = jobCategoryRepo;
            _jobTypeRepo = jobTypeRepo;
            _applicationUserRepo = applicationUserRepo;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<IList<JobOffer>> GetAllOffers()
        {
            return await _jobOfferRepo.GetAll();
        }

        public async Task<JobOffer> GetOfferById(string id)
        {
            return await _jobOfferRepo.GetById(id);
        }

        public async Task<bool> Add(JobOffer item)
        {
            item.Submitted = DateTime.Now;
            item.LastEdit = DateTime.Now;

            _jobOfferRepo.Add(item);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Edit(JobOffer item)
        {
            var offer = await _jobOfferRepo.GetById(item.JobOfferId);
            offer.JobCategory = await _jobCategoryRepo.GetById(item.JobCategoryId);
            offer.JobType = await _jobTypeRepo.GetById(item.JobTypeId);
            offer.PostalCode = item.PostalCode;
            offer.Title = item.Title;
            offer.Description = item.Description;
            offer.Wage = item.Wage;
            offer.LastEdit = DateTime.Now;

            try
            {
                _jobOfferRepo.Update(offer);
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(JobOffer item)
        {
            _jobOfferRepo.Delete(item);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<IEnumerable<JobOffer>> GetMostPopularOffers()
        {
            return (await _jobOfferRepo.GetAll())
                .OrderByDescending(m => m.Visits)
                .Take(5);
        }

        public async Task<IEnumerable<JobOffer>> GetOffersContainingPhrase(string phrase)
        {
            phrase = phrase.ToLower();
            var offers = await _jobOfferRepo.GetAll();
            return offers.Where(c => c.PostalCode.ToLower().Contains(phrase)
                                     ||c.Title.ToLower().Contains(phrase)
                                     || c.Description.ToLower().Contains(phrase)
                                     || c.JobType.Name.ToLower().Contains(phrase)
                                     || c.JobCategory.Name.ToLower().Contains(phrase)
                                     || c.Author.Email.ToLower().Contains(phrase));
        }

        public async Task<bool> CanUserEditOffer(string userId, string offerId)
        {
            var user = await _applicationUserRepo.GetById(userId);
            var offer = await _jobOfferRepo.GetById(offerId);

            return offer.Author.Id == user.Id
                   || await _authService.IsInRole(user, RoleHelper.Administrator)
                   || await _authService.IsInRole(user, RoleHelper.Moderator);
        }

        public async Task<bool> IncreaseOfferViews(JobOffer offer)
        {
            offer.Visits += 1;
            _jobOfferRepo.Update(offer);
            await _unitOfWork.Save();
            return true;
        }
    }
}
