using Microsoft.AspNetCore.Authentication;
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
        private readonly IRecommendationRepository<Review> _reviewRepository;

        public HomeController(ILogger<HomeController> logger, IRecommendationRepository<User> userRepository,
            IRecommendationRepository<Review> reviewRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet("")]
        [HttpGet("Home")]
        public IActionResult Index()
        {
            return View(_userRepository.GetValues);
        }

        [HttpGet("Registration")]
        public IActionResult Registration() => View();
        

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
            if (!ModelState.IsValid) 
                return View("LogIn", userLogin);
            
            var user = _userRepository.Authenticate(userLogin);

            if (user == null)
                return RedirectToAction("Error", new { message = "Wrong email or password. Try again or register." });
            
            await UserSingIn(user);
            return RedirectToAction(user.Status.ToString() == "Admin" ? "AdminPanel" : "UserPanel", new { user.Id });
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
            
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpGet("UserPanel/{Id:guid}")]
        public IActionResult UserPanel(Guid Id)
        {
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Id == Id);

            var userReviews = _reviewRepository.GetValues.Where(x => x.UserId == Id);

            return View(userReviews);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("AdminPanel/{Id:guid}")]
        public IActionResult AdminPanel(Guid Id)
        {
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Id == Id);
            var users = _userRepository.GetValues.Where(x => x.Status == Models.User.StatusType.User);

            return View(users);
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
            
            return User.Claims.Skip(1).First().Value == "Admin" ? RedirectToAction("AdminPanel", new { Id = id}) 
                : RedirectToAction("UserPanel", new { id });
        }

        public async Task UserSingIn(User user)
        {
            var claims = GetClaims(user);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> SearchUser(string Id)
        {
            try
            {
                var id = Guid.Parse(Id);
                var user = _userRepository.GetValues.First(x => x.Id == id);
                await UserSingIn(user);
                
                return RedirectToAction("UserPanel", new {user.Id});
            }
            catch (ArgumentNullException ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message, this);
                return RedirectToAction("Error", new { message = "Something went wrong" });
            }
            catch (FormatException ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message, this);
                return RedirectToAction("Error", new {  message = "Something went wrong" });
            }
        }
    }
}