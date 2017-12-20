using AutoMapper;
using BulletinBoard.Models.JobCategoryViewModels;
using BulletinBoard.Models.JobTypeViewModels;

namespace BulletinBoard.Models.Mappings
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<JobTypeViewModel, JobType>();
            CreateMap<CreateJobTypeViewModel, JobType>();
            CreateMap<DeleteJobTypeViewModel, JobType>();
            CreateMap<DetailsJobTypeViewModel, JobType>();
            CreateMap<EditJobTypeViewModel, JobType>();

            CreateMap<JobCategoryViewModel, JobCategory>();
            CreateMap<CreateJobCategoryViewModel, JobCategory>();
            CreateMap<DeleteJobCategoryViewModel, JobCategory>();
            CreateMap<DetailsJobCategoryViewModel, JobCategory>();
            CreateMap<EditJobCategoryViewModel, JobCategory>();
        }
    }
}
