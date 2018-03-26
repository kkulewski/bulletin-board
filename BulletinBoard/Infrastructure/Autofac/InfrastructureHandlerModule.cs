using Autofac;
using AutoMapper;

namespace BulletinBoard.Infrastructure.Autofac
{
    public class InfrastructureHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>();
        }
    }
}
