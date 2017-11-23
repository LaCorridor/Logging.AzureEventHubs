Logging.EventHubs
-

Logging.EventHubs implements logger providers for .NET Core 2.0 applications. It intends to provide a simple way to write .NET Core application logs into EventHubs.

# Getting Started
## Install the pcakge
* Install the Nuget Package by .NET CLI:
```
dotnet add package LaCorridor.Logging.AzureEventHubs
```
Or
* Install the Nuget Package by Package Manager:
```
Install-Package LaCorridor.Logging.AzureEventHubs
```

## Inject the logger
* For .NET Core 2.0 Websites, Update `BuildWebHost` method in Program.cs, appending calling to `ConfgureLogging()`:
```csharp
using LaCorridor.Logging.AzureEventHubs;

namespace WebExample
{
    public class Program
    {
        // ... Main methods ...
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.AddEventHub(
                        "EventHub connection string",
                        LogLevel.Warning);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
```

* For .NET Core 1.x Websites, Update `Configure` method in 'Startup.cs':
```csharp
using LaCorridor.Logging.AzureEventHubs;
// ...
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();
    loggerFactory.AddEventHub(
        "EvetHub connection string", 
        LogLevel.Debug
    );
    // ...
}
```
* For .NET Core Console Application, there are various of ways to do the injection. Here's an example:
```csharp
class Program
{
    static void Main(string[] args)
    {
        ServiceCollection services = new ServiceCollection();
        services.AddTransient<Runner>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug)
            .AddEventHub("EventHubs connection string", LogLevel.Debug));

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        Runner runner = serviceProvider.GetService<Runner>();
        runner.Run();
        
        Console.WriteLine("Press any key to continue . . .");
        Console.ReadKey(true);
    }
}
```
Runner is a class that consumes ILogger<Runner>:
```csharp
public class Runner
{
    ILogger _logger;
    public Runner(ILogger<Runner> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogDebug("A Debug Message");
    }
}
```

## Consumes the logger

* Get the ILogger service, for example, in HomeController:
```csharp
public class HomeController : Controller
{
    private readonly ILogger _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // ...
}
```

* Use the logger:
```csharp
public class HomeController : Controller
{
    // ...
    public IActionResult Index()
    {
        _logger.LogWarning("Entering Index.");
        return View();
    }
    // ...
}
```

# Result Example:
![Example Result Image](https://github.com/LaCorridor/Logging.AzureEventHubs/blob/master/Assets/ExampleResult.png)
