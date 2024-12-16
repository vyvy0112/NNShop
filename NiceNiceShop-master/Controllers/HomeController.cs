using Microsoft.AspNetCore.Mvc;
using NNShop.Models;
using System.Diagnostics;

namespace NNShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/sorry")]
        public IActionResult Sorry()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult LienHe()
        {
            return View();
        }
    }
}
