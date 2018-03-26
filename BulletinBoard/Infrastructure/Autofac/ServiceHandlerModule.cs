using Autofac;
using BulletinBoard.Services;
using BulletinBoard.Services.Abstract;

namespace BulletinBoard.Infrastructure.Autofac
{
    public class ServiceHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SeedService>().As<ISeedService>();

            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<JobCategoryService>().As<IJobCategoryService>();
            builder.RegisterType<JobTypeService>().As<IJobTypeService>();
            builder.RegisterType<JobOfferService>().As<IJobOfferService>();
        }
    }
}
