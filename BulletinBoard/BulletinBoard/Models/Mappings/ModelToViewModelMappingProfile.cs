using AutoMapper;
using BulletinBoard.Models.JobTypeViewModels;

namespace BulletinBoard.Models.Mappings
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<CreateJobTypeViewModel, JobType>();
            CreateMap<DeleteJobTypeViewModel, JobType>();
            CreateMap<DetailsJobTypeViewModel, JobType>();
            CreateMap<EditJobTypeViewModel, JobType>();
        }
    }
}
