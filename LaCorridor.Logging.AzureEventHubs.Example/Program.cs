using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace LaCorridor.Logging.AzureEventHubs.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory factory = new LoggerFactory();
            // Replace the argument with your own event hub conneciton string.
            factory.AddEventHub("Your Eventhub String");
            factory.AddConsole(LogLevel.Warning);

            ILogger logger = factory.CreateLogger<Program>();

            logger.LogWarning("This is a warning. Expect to show up in console and event hub at the same time");

            // Need sometime for the data to populate through the pipeline.
            Thread.Sleep(10000);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }
    }
}
