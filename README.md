Logging.EventHubs
-

Logging.EventHubs implements logger providers for .NET Core 2.0 applications. It intends to provide a simple way to write .NET Core application logs into EventHubs.

# Getting Started
* Install the Nuget Package:
```
Install-Package LaCorridor.Logging.AzureEventHubs -Prerelease
```

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
                        "Endpoint=sb://namespace;SharedAccessKeyName=Send;SharedAccessKey=abcdefg=;EntityPath=eventhubname",
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
...
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();
    loggerFactory.AddEventHubLogger("string", LogLevel.Debug);
    // ...
}
```

* Inject ILogger, for example, in HomeController:
```csharp
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        ...
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
```

# Result Example:
![Example Result Image](https://github.com/LaCorridor/Logging.AzureEventHubs/blob/master/Assets/ExampleResult.png)
