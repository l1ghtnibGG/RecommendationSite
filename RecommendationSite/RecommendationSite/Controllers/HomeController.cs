using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.Repo;
using System.Security.Claims;
using Korzh.EasyQuery.Linq;
using Sparc.TagCloud;

namespace RecommendationSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecommendationRepository<User> _userRepository;
        private readonly IRecommendationRepository<Review> _reviewRepository;
        private readonly IRecommendationRepository<Tag> _tagRepository;

        public HomeController(ILogger<HomeController> logger, IRecommendationRepository<User> userRepository,
            IRecommendationRepository<Review> reviewRepository, IRecommendationRepository<Tag> tagRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet("")]
        [HttpGet("Home")]
        public IActionResult Index(string tag)
        {
            IQueryable<Review> reviews;

            if (tag == null)
                reviews = _reviewRepository.GetValues;
            else
                reviews =
                    from review in _reviewRepository.GetValues
                    where review.Tags.Any(x => x.Name == tag)
                    select review;

            return View(new Tuple<IQueryable<Review>, IEnumerable<TagCloudTag>>(reviews, CreateTagCloud()));
        }
        
        [HttpPost("Home")]
        public IActionResult IndexPost(string reviewSearch)
        {
            IQueryable<Review> reviews;

            if (!string.IsNullOrEmpty(reviewSearch))
                reviews = _reviewRepository.GetValues.FullTextSearchQuery(reviewSearch);
            else
                reviews = _reviewRepository.GetValues;

            return View("Index",new Tuple<IQueryable<Review>, IEnumerable<TagCloudTag>>(reviews, CreateTagCloud()));
        }

        private IEnumerable<TagCloudTag> CreateTagCloud()
        {
            var rightName = _tagRepository.GetValues.Select(x => x.Name).ToList();
            
            var tags = new TagCloudAnalyzer()
                .ComputeTagCloud(_tagRepository.GetValues.Select(x => x.Name))
                .Shuffle();

            var count = 0;
            foreach (var tag in tags)
            {
                tag.Text = rightName[count];
                count++;
            }

            return tags;
        }

        [HttpGet("Registration")]
        public IActionResult Registration() => View();
        

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromForm] UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.GetValues.FirstOrDefault(x => 
                        x.Email == userRegistration.EmailAddress) == null)
                {
                    return await CreateUser(new User
                    {
                        Email = userRegistration.EmailAddress,
                        Name = userRegistration.Name,
                        Password = userRegistration.Password
                    });
                }

                return RedirectToAction("Error", new { message = 
                    "User already exist. Try again or log in." });
            }

            return View(userRegistration);
        }

        private async Task<IActionResult> CreateUser(User user)
        {
            var createdUser = _userRepository.Add(user);

            await UserSingIn(createdUser);
            return RedirectToAction("UserPanel", new {createdUser.Id});
        }

        [HttpGet("LogIn")]
        public IActionResult LogIn() => View();

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm]UserLogIn userLogin)
        {
            if (!ModelState.IsValid) 
                return View(userLogin);
            
            var user = Authenticate(userLogin);

            if (user == null)
                return RedirectToAction("Error", new { message = 
                    "Wrong email or password. Try again or register." });
            

            await UserSingIn(user);
            return RedirectToAction(user.Status.ToString() == "Admin" ? 
                "AdminPanel" : "UserPanel", new { user.Id });
        }

        public async Task SignInSocialNet(string social)
        {
            await HttpContext.ChallengeAsync(social, new AuthenticationProperties()
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

            var userLogIn = new UserLogIn()
            {
                EmailAddress = claims.Last().Value,
                Password = claims.First().Value
            };

            var user = Authenticate(userLogIn) ?? _userRepository.Add(new User
            {
                Email = userLogIn.EmailAddress,
                Name = claims.Skip(2).First().Value,
                Password = userLogIn.Password
            });

            return RedirectToAction("UserPanel", new { email = user.Email });
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = MicrosoftAccountDefaults.AuthenticationScheme)]
        [HttpGet("UserPanel/{email}")]
        public IActionResult UserPanel(string email) => View( _reviewRepository
            .GetValues.Where(x => x.User.Email == email));

        [Authorize(Roles = "User")]
        [HttpGet("UserPanel/{Id:guid}")]
        public IActionResult UserPanel(Guid Id) => View(_reviewRepository
            .GetValues.Where(x => x.UserId == Id));
        
        [Authorize(Roles = "Admin")]
        [HttpGet("AdminPanel/{Id:guid}")]
        public IActionResult AdminPanel(Guid Id) => View(_userRepository
            .GetValues.Where(x => x.Status == Models.User.StatusType.User));

        public IActionResult DeleteUser(string Id)
        {
            try
            {
                var id = Guid.Parse(Id);

                if (_userRepository.Delete(id) != "Wrong")
                {
                    return RedirectToAction("AdminPanel", new
                    {
                        Id = User.Claims.First().Value
                    });
                }
                
                return RedirectToAction("Error", new { message = 
                    "User's id is wrong" });
            }
            catch (FormatException ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message);
                return RedirectToAction("Error", new { message = 
                    "User's id is wrong" });
            }
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
                var id = _userRepository.GetValues.First(x => 
                    x.Email == User.Claims.Last().Value).Id.ToString();
                var userEmail = User.Claims.Last().Value;

                return RedirectToAction("UserPanel", new { email = userEmail});
            }
            
            return SelectPanel();
        }

        public IActionResult SelectPanel()
        {
            var id = User.Claims.First().Value;
            
            return User.Claims.Skip(1).First().Value == "Admin" ? 
                RedirectToAction("AdminPanel", new { id }) : 
                RedirectToAction("UserPanel", new { id });
        }

        public async Task UserSingIn(User user)
        {
            var claims = GetClaims(user);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));
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
                return RedirectToAction("Error", new { message = 
                    "Something went wrong" });
            }
            catch (FormatException ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message, this);
                return RedirectToAction("Error", new {  message = 
                    "Something went wrong" });
            }
        }
        
        private User? Authenticate(UserLogIn userLogin) => _userRepository.
            GetValues.FirstOrDefault(x => x.Email == userLogin.EmailAddress && 
                                          x.Password == userLogin.Password) ?? null;
    }
}