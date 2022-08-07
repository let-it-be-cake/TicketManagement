using Autofac;
using AutoMapper;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.ServiceTables;

namespace ThirdPartyEventEditor.App_Start.DIModules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ThirdPartyEventViewModel, Event>();
                cfg.CreateMap<Event, ThirdPartyEventViewModel>();
            }))
            .AsSelf()
            .SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }
}