using System.Linq;
using Mediator.Net.MicrosoftDependencyInjection;
using Mediator.Net.TestUtil;
using Mediator.Net.TestUtil.Handlers.RequestHandlers;
using Mediator.Net.TestUtil.Services;
using Mediator.Net.WebApiSample.Handlers.CommandHandler;
using Mediator.Net.WebApiSample.Handlers.EventHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Net.WebApiSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMediator(services);
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        void ConfigureMediator(IServiceCollection services)
        {
            services
                .AddTransient<SimpleService>()
                .AddTransient<AnotherSimpleService>()
                .AddTransient<ICalculateService, CalculateService>()
                .AddTransient<IBoardcastService, BoardcastService>();
            var mediatorBuilder = new MediatorBuilder();
            mediatorBuilder.RegisterHandlers(assembly => assembly.DefinedTypes.Where(t => t.Name != nameof(MultiRequestsHandler)), TestUtilAssembly.Assembly, typeof(Startup).Assembly);
            services.RegisterMediator(mediatorBuilder);
        }
    }
}
