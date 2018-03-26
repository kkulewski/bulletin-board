using Autofac;
using AutoMapper;
using BulletinBoard.Data;

namespace BulletinBoard.Infrastructure.Autofac
{
    public class InfrastructureHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>();
            builder.RegisterType<ApplicationDbInitializer>();
        }
    }
}
