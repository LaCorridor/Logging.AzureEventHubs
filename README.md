Logging.EventHubs
-

Logging.EventHubs implements logger providers for .NET Core 2.0 applications. It intends to provide a simple way to write .NET Core application logs into EventHubs.

# Getting Started
## Install the package
* Install the NuGet Package by .NET CLI:
```
dotnet add package LaCorridor.Logging.AzureEventHubs
```
Or
* Install the NuGet Package by Package Manager:
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

## Use configuration file
Providing connection string through configuration file is supported. Take .NET Core 2.0 ASP.NET Website as an example, Program.cs:
```csharp
public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging((hostContext, builder) =>
        {
            builder.AddEventHub(hostContext.Configuration);
        })
        .UseStartup<Startup>()
        .Build();
```
Example of appsettings.json:
```json
{
  "Logging": {
    "IncludeScopes": true,
    "LogLevel": {
      "Default": "Debug"
    },
    "EventHubs": {
      "ConnectionString": "EventHubs connection string",
      "LogLevel": "Error"
    }
  }
}

```
**Details for settings:**

| Name | Type | Required | Value | Description |
| -----|------|:--------:|-------|-------------|
|ConnectionString| String | yes | EventHubs Connection String |EventHubs connection you can get from the Azure portal or the [EventHub Viewer]("https://www.microsoft.com/en-us/store/p/eventhub-viewer/9nblggh4wnmd"). |
|LogLevel| Enum |   no | One of the following: `Trace` \| `Debug` \| `Information` \| `Warning` \| `Error` \| `Critical` \| `None` | Minimal logging level. In those values, `Trace` provides most chatty logs while `None` provides no logs. |

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
