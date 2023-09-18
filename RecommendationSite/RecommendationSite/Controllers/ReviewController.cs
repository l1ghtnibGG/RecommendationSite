using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.Repo;

namespace RecommendationSite.Controllers;

public class ReviewController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRecommendationRepository<Review> _reviewRepository;
    private readonly IRecommendationRepository<Comment> _commentRepository;
    private readonly IRecommendationRepository<User> _userRepository;
    public ReviewController(ILogger<HomeController> logger, IRecommendationRepository<Review> reviewRepository,
        IRecommendationRepository<Comment> commentRepository, IRecommendationRepository<User> userRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    [HttpGet("/Review/{Id}")]
    public IActionResult ReviewPanel(string Id)
    {
        var review = SearchReview(Id);

        if (review == null)
                return RedirectToAction("Error", "Home", new {message = "Review not found"});

        var commentUser = 
            (from comment in _commentRepository.GetValues.Where(x => x.ReviewId == review.Id)
            join user in _userRepository.GetValues on comment.UserId equals user.Id
            select new
            {
                Comments = comment,
                UserEmail = user.Email
            }).AsEnumerable().Select(x => (x.Comments, x.UserEmail));

        return View("ReviewPanel",  new Tuple<Review, IEnumerable<(Comment, string)>>(review, commentUser));
    }

    private Review SearchReview(string Id)
    {
        try
        {
            var id = Guid.Parse(Id);
            var review = _reviewRepository.GetValues.First(x => x.Id == id);

            return review;
        }
        catch (FormatException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message, this);
            return null!;
        }
    }

    [HttpGet("Review/Add")]
    public IActionResult AddReview() => View();

    [HttpPost("Review/Add")]
    public IActionResult AddReview([FromForm] Review review)
    {
        var rev = review;
        return RedirectToAction("UserPanel", "Home");
    }
}