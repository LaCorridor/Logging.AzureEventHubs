using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace LaCorridor.Logging.AzureEventHubs.Example.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, builder) =>
                {
                    builder.AddEventHub(hostContext.Configuration);
                    builder.AddConsole(options =>
                    {
                        options.IncludeScopes = true;
                    });
                })
                .UseStartup<Startup>()
                .Build();
    }
}
