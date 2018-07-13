using Microsoft.AspNetCore.Hosting;

namespace Mediator.Net.WebApiSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => CreateWebHostBuilder(args).Build();

        /// <summary>
        /// This method is used by the Mediator.Net.WebApiSample.Test project
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>();
        }
    }
}
