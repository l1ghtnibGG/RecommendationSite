using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.RegistrationModels;
using RecommendationSite.Models.Repo;

namespace RecommendationSite.Controllers;

public class CommentController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRecommendationRepository<User> _userRepository;
    private readonly IRecommendationRepository<Review> _reviewRepository;
    private readonly IRecommendationRepository<Comment> _commentRepository;

    public CommentController(ILogger<HomeController> logger, IRecommendationRepository<User> userRepository,
        IRecommendationRepository<Review> reviewRepository, IRecommendationRepository<Comment> commentRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _commentRepository = commentRepository;
    }

    [HttpGet("Comment/Add")]
    public IActionResult AddComment(string reviewId, string userEmail)
    {
        try
        {
            var id = Guid.Parse(reviewId);
            
            var user = _userRepository.GetValues.FirstOrDefault(x => x.Email == userEmail);

            if (user == null) 
                return RedirectToAction("Error", "Home", new { message = "Something went wrong. try again" });
        
            var commentAdd = new CommentAddModel()
            {
                ReviewId = id,
                UserId = user.Id
            };
            
            return View(commentAdd);
        }
        catch (FormatException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message);
            return RedirectToAction("Error", "Home", new { message = 
                "Review's id is wrong" });
        }
    }

    [HttpPost("Comment/Add")]
    public IActionResult AddComment([FromForm] CommentAddModel commentAddModel)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("ReviewPanel", "Review", new
            {
                Id = _commentRepository.Add(CreateComment(commentAddModel)).ReviewId.ToString()
            });
        }

        return View(commentAddModel);
    }

    private Comment CreateComment(CommentAddModel commentAddModel) =>
        new Comment
        {
            Title = commentAddModel.Title,
            Text = commentAddModel.Text,
            CreateDate = DateTime.Now,
            ReviewId = commentAddModel.ReviewId,
            UserId = commentAddModel.UserId
        };
}