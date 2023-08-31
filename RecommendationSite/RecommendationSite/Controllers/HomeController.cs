using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecommendationSite.Models;
using RecommendationSite.Models.Repo;
using RecommendationSite.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

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

        [HttpGet("")]
        [HttpGet("Home")]
        public IActionResult Index()
        {
            return View(_userRepository.GetValues);
        }

        [HttpGet("Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost("Registration")]
        public IActionResult Registration([FromForm] UserRegistration user)
        {
            if (ModelState.IsValid)
            {

            }

            return View(user);
        }

        [HttpGet("LogIn")]
        public IActionResult LogIn() => View();

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm]UserLogIn userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.Authenticate(userLogin);

                if (user != null)
                {
                    var claims = GetClaims(user);

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (user.Status.ToString() == "Admin")
                    {
                        return RedirectToAction("AdminPanel", new { Id = user.Id });
                    }

                    return RedirectToAction("UserPanel", new {Id = user.Id});
                }

                return RedirectToAction("Error", new { message = "Wrong email or password. Try again or registrate."});
            }

            return View("LogIn", userLogin);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        [HttpGet("UserPanel/{Id}")]
        public IActionResult UserPanel(Guid Id)
        {
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Id == Id);

            return View(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("AdminPanel/{Id}")]
        public IActionResult AdminPanel(Guid Id)
        {
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Id == Id);

            return View(user);
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error(string message) => View("Error", message);

        public List<Claim> GetClaims(User user) => new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Status.ToString())
        };
    }
}