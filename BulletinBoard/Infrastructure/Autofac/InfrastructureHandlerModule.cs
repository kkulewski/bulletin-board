using Autofac;
using AutoMapper;
using BulletinBoard.Services;
using BulletinBoard.Services.Abstract;

namespace BulletinBoard.Infrastructure.Autofac
{
    public class InfrastructureHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>();
            builder.RegisterType<SeedService>().As<ISeedService>();
        }
    }
}
