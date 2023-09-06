﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
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
                if (_userRepository.GetValues.FirstOrDefault(x => x.Email == user.EmailAddress) == null)
                {
                    _userRepository.Add(user);

                    return RedirectToAction("UserPanel");
                }

                return RedirectToAction("Error", new { message = "User already exist. Try again or log in." });
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

                    return RedirectToAction(user.Status.ToString() == "Admin" ? "AdminPanel" : "UserPanel", new { user.Id });
                }

                return RedirectToAction("Error", new { message = "Wrong email or password. Try again or registrate."});
            }

            return View("LogIn", userLogin);
        }

        public async Task SignInGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("SignInBySocial")
            });
        }

        public async Task SignInMicrosoft()
        {
            await HttpContext.ChallengeAsync(MicrosoftAccountDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("SignInBySocial")
            });
        }

        public async Task<IActionResult> SignInBySocial()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                    .First().Claims.Select(claim => new
                    {
                        claim.Type,
                        claim.Value
                    });
            
            var id = claims.First().Value;
            var email = claims.Last().Value;

            return RedirectToAction("UserPanel", new {Id = id, Email = email});
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = MicrosoftAccountDefaults.AuthenticationScheme)]
        [HttpGet("UserPanel/{Id}")]
        public IActionResult UserPanel(string Id, string Email)
        {
            var user = new User()
            {
                Email = Email,
                Status = Models.User.StatusType.User
            };
            
            return View(user);
        }

        [Authorize(Roles = "User")]
        [HttpGet("UserPanel/{Id:guid}")]
        public IActionResult UserPanel(Guid Id)
        {
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Id == Id);

            return View(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("AdminPanel/{Id:guid}")]
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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Status.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        public IActionResult CheckRole()
        {
            if (User.Identity.AuthenticationType != "Cookies")
            {
                var id = User.Claims.First().Value;
                var email = User.Claims.Last().Value;

                return RedirectToAction("UserPanel", new { Id = id, Email = email });
            }
            
            return SelectPanel();
        }

        public IActionResult SelectPanel()
        {
            var id = User.Claims.First().Value;
            
            if (User.Claims.Skip(1).First().Value == "Admin")
            {
                return RedirectToAction("AdminPanel", new { Id = id});
            }

            return RedirectToAction("UserPanel", new { id });
        }
    }
}