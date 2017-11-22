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
                .ConfigureLogging(builder =>
                {
                    builder.AddEventHub(
                        "You Eventhub Connection String",
                        LogLevel.Debug);
                    builder.AddConsole(options =>
                    {
                        options.IncludeScopes = true;
                    });
                })
                .UseStartup<Startup>()
                .Build();
    }
}
