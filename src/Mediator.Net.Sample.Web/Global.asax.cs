using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Mediator.Net.Autofac;
using Mediator.Net.Sample.Web.Controllers;
using Mediator.Net.Sample.Web.Services;

namespace Mediator.Net.Sample.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register MVC controllers
            builder.RegisterControllers(typeof(HomeController).Assembly);

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);
            var mediatorBuilder = new MediatorBuilder();
            mediatorBuilder.RegisterHandlers(Assembly.GetExecutingAssembly());
            builder.RegisterMediator(mediatorBuilder);

            // Misc
            builder.RegisterType<SimpleService>().AsImplementedInterfaces();
        

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            var inner = container.BeginLifetimeScope();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(inner);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(inner));


        }
    }
}
