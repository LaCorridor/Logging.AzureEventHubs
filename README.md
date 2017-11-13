Logging.EventHubs
-

Logging.EventHubs implements logger providers for .NET Core 2.0 applications. It intends to provide a simple way to write .NET Core application logs into EventHubs.

# Getting Started
* In ASP.NET Core 2.0 WebSites, add reference to nuget package:
```
Install-Package LaCorridor.Logging.AzureEventHubs -Prerelease
```

* Update `BuildWebHost` method in Program.cs, appending calling to `ConfgureLogging()`:
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
