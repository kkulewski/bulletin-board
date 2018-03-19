﻿using AutoMapper;
using BulletinBoard.Models;
using BulletinBoard.Models.JobCategoryViewModels;
using BulletinBoard.Models.JobOfferViewModels;
using BulletinBoard.Models.JobTypeViewModels;

namespace BulletinBoard.Infrastructure
{
    /// <summary>
    /// Default model to view-model automapper mapping profile.
    /// </summary>
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            #region JobCategory

            CreateMap<JobCategory, JobCategoryViewModel>();
            CreateMap<JobCategory, CreateJobCategoryViewModel>();
            CreateMap<JobCategory, DeleteJobCategoryViewModel>();
            CreateMap<JobCategory, DetailsJobCategoryViewModel>();
            CreateMap<JobCategory, EditJobCategoryViewModel>();

            CreateMap<CreateJobCategoryViewModel, JobCategory>();
            CreateMap<DeleteJobCategoryViewModel, JobCategory>();
            CreateMap<DetailsJobCategoryViewModel, JobCategory>();
            CreateMap<EditJobCategoryViewModel, JobCategory>();

            #endregion

            #region JobType
            
            CreateMap<JobType, JobTypeViewModel>();
            CreateMap<JobType, CreateJobTypeViewModel>();
            CreateMap<JobType, DeleteJobTypeViewModel>();
            CreateMap<JobType, DetailsJobTypeViewModel>();
            CreateMap<JobType, EditJobTypeViewModel>();

            #endregion

            #region JobOffer

            CreateMap<JobOffer, JobOfferViewModel>();
            CreateMap<JobOffer, PopularJobOfferViewModel>();
            CreateMap<JobOffer, CreateJobOfferViewModel>()
                .ForMember(dest => dest.JobCategoryId,
                    opts => opts.MapFrom(src => src.JobCategory.JobCategoryId))
                .ForMember(dest => dest.JobTypeId,
                    opts => opts.MapFrom(src => src.JobType.JobTypeId));
            CreateMap<JobOffer, DeleteJobOfferViewModel>();
            CreateMap<JobOffer, DetailsJobOfferViewModel>();
            CreateMap<JobOffer, EditJobOfferViewModel>()
                .ForMember(dest => dest.JobCategoryId,
                    opts => opts.MapFrom(src => src.JobCategory.JobCategoryId))
                .ForMember(dest => dest.JobTypeId,
                    opts => opts.MapFrom(src => src.JobType.JobTypeId));

            #endregion
        }
    }
}
