using AutoMapper;
using BulletinBoard.Infrastructure.AutoMapper;
using Xunit;

namespace BulletinBoard.Tests.Infrastructure
{
    public class AutoMapperConfigurationTests
    {
        [Fact (Skip = "Mapping profile is not finished yet")]
        public void Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DefaultProfile()));
            config.AssertConfigurationIsValid();
        }
    }
}
