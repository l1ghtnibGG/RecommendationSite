using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.Repo;
using System.Diagnostics;

namespace RecommendationSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecommendationRepository<User> _userRepository;

        public HomeController(ILogger<HomeController> logger, IRecommendationRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(_userRepository.GetValues);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}