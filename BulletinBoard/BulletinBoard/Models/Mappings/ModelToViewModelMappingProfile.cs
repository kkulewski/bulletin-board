using AutoMapper;
using BulletinBoard.Models.JobCategoryViewModels;
using BulletinBoard.Models.JobOfferViewModels;
using BulletinBoard.Models.JobTypeViewModels;

namespace BulletinBoard.Models.Mappings
{
    /// <summary>
    /// Default model to view-model automapper mapping profile.
    /// </summary>
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<JobType, JobTypeViewModel>();
            CreateMap<JobType, CreateJobTypeViewModel>();
            CreateMap<JobType, DeleteJobTypeViewModel>();
            CreateMap<JobType, DetailsJobTypeViewModel>();
            CreateMap<JobType, EditJobTypeViewModel>();

            CreateMap<JobCategory, JobCategoryViewModel>();
            CreateMap<JobCategory, CreateJobCategoryViewModel>();
            CreateMap<JobCategory, DeleteJobCategoryViewModel>();
            CreateMap<JobCategory, DetailsJobCategoryViewModel>();
            CreateMap<JobCategory, EditJobCategoryViewModel>();

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
        }
    }
}
