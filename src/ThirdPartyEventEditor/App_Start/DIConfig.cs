using System.Web.Mvc;
using Autofac;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using ThirdPartyEventEditor.App_Start.DIModules;

namespace ThirdPartyEventEditor
{
    public static class DIConfig
    {
        public static void RegisterDI(string jsonPath)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ImplementationServiceModule(jsonPath));
            builder.RegisterModule(new AutoMapperModule());

            builder.RegisterModule<NLogModule>();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(Global).Assembly);

            // Register model binders that require DI.
            builder.RegisterModelBinders(typeof(Global).Assembly);
            builder.RegisterModelBinderProvider();

            // Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}