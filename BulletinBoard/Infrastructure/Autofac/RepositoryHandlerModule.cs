using Autofac;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories;
using BulletinBoard.Data.Repositories.Abstract;

namespace BulletinBoard.Infrastructure.Autofac
{
    public class RepositoryHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MigrationManager>().As<IMigrationManager>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<JobCategoryRepository>().As<IJobCategoryRepository>();
            builder.RegisterType<JobTypeRepository>().As<IJobTypeRepository>();
            builder.RegisterType<JobOfferRepository>().As<IJobOfferRepository>();
        }
    }
}
