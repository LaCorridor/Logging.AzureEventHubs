using System;
using System.Diagnostics;
using LaCorridor.Logging.AzureEventHubs.Example.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LaCorridor.Logging.AzureEventHubs.Example.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private IDisposable _scope;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            // TODO: Deal with scope leaking.
            _scope = logger.BeginScope("Hello");
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Entering Index.");
            return View();
        }

        public IActionResult About()
        {
            _logger.LogDebug("Entering About.");

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            _logger.LogDebug("Entering Contact.");

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            _logger.LogError("There is an error.");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
