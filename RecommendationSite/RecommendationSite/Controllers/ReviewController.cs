using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.Repo;

namespace RecommendationSite.Controllers;

public class ReviewController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRecommendationRepository<Review> _reviewRepository;

    public ReviewController(ILogger<HomeController> logger, IRecommendationRepository<Review> reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    [HttpGet("/Review/{Id}")]
    public IActionResult ReviewPanel(string Id)
    {
        var review = SearchReview(Id);

        if (review == null)
        {
            return RedirectToAction("Error", "Home", new {message = "Review not found"});
        }
        
        return View("ReviewPanel", review);
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