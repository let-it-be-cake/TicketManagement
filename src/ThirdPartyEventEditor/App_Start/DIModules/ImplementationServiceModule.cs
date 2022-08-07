using Autofac;
using ThirdPartyEventEditor.Services;
using ThirdPartyEventEditor.Services.Interfaces;
using ThirdPartyEventEditor.ServiceTables;

namespace ThirdPartyEventEditor.App_Start.DIModules
{
    public class ImplementationServiceModule : Module
    {
        private readonly string _connectionString;

        public ImplementationServiceModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(o => new EventService(_connectionString)).As<IProxyService<Event>>();
        }
    }
}