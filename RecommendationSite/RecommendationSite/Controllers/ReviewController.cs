using Microsoft.AspNetCore.Mvc;
using RecommendationSite.Models;
using RecommendationSite.Models.RegistrationModels;
using RecommendationSite.Models.Repo;

namespace RecommendationSite.Controllers;

public class ReviewController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRecommendationRepository<Review> _reviewRepository;
    private readonly IRecommendationRepository<Comment> _commentRepository;
    private readonly IRecommendationRepository<User> _userRepository;
    private readonly IRecommendationRepository<Score> _scoreRepository;
    public ReviewController(ILogger<HomeController> logger, IRecommendationRepository<Review> reviewRepository,
        IRecommendationRepository<Comment> commentRepository, IRecommendationRepository<User> userRepository,
        IRecommendationRepository<Score> scoreRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _scoreRepository = scoreRepository;
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
    public IActionResult AddReview(string email)
    {
        var user = _userRepository.GetValues.FirstOrDefault(x => x.Email == email);

        if (user == null) 
            return RedirectToAction("Error", "Home", new { message = "Something went wrong. try again" });
        
        var reviewAddModel = new ReviewAddModel()
        {
            Id = Guid.Empty,
            UserId = user.Id
        };
            
        return View(reviewAddModel);
    } 

    [HttpPost("Review/Add")]
    public IActionResult AddReview([FromForm] ReviewAddModel reviewAddModel)
    {
        if (ModelState.IsValid && IsReviewValid(reviewAddModel))
        {
            if (reviewAddModel.IsAdd == "1")
            {
                if (_reviewRepository.GetValues.FirstOrDefault(x => 
                        x.Title == reviewAddModel.Title && 
                        x.Group == reviewAddModel.Group) == null)
                {
                    return RedirectToAction("ReviewPanel", new 
                    {
                        Id = _reviewRepository.Add(CreateReview(reviewAddModel)).Id.ToString()
                        
                    });
                }
                
                return RedirectToAction("Error", "Home", new { message = 
                    "Review already exist" });
            }
            
            return RedirectToAction("ReviewPanel", new
            {
                Id = _reviewRepository.Edit(CreateReview(reviewAddModel)).Id.ToString()
            });
        }

        return View(reviewAddModel);
    }

    private Review CreateReview(ReviewAddModel reviewAddModel) => 
        new Review
        {
            Id = reviewAddModel.Id,
            Title = reviewAddModel.Title,
            Name = reviewAddModel.Name,
            Group = reviewAddModel.Group,
            ImageUrl = UploadImage(reviewAddModel.ImageUrl).Result,
            Mark = reviewAddModel.Mark,
            Text = reviewAddModel.Text,
            UserId = reviewAddModel.UserId
        };


    [HttpGet("Review/Edit/{Id}")]
    public IActionResult EditReview(string Id)
    {
        try
        {
            var id = Guid.Parse(Id);
            var review = _reviewRepository.GetValues.First(x => x.Id == id);

            var reviewAddModel = new ReviewAddModel
            {
                Id = review.Id,
                Title = review.Title,
                Name = review.Name,
                Group = review.Group,
                Mark = review.Mark,
                Text = review.Text,
                UserId = review.UserId
            };
            
            return View(reviewAddModel);
        }
        catch (FormatException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message);
            return RedirectToAction("Error", "home", new { message = 
                "Review's id is wrong" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message);
            return RedirectToAction("Error", "home", new { message = 
                "User not found by id" });
        }
    }

    public IActionResult DeleteReview(string Id)
    {
        try
        {
            var id = Guid.Parse(Id);
            var userId = _reviewRepository.GetValues.First(x => x.Id == id).UserId;
            
            if (_reviewRepository.Delete(id) != "Wrong")
            {
                return RedirectToAction("UserPanel", "Home", new
                {
                    Id = userId
                });
            }
            
            return RedirectToAction("Error", "home", new { message = 
                "Review's id is wrong" });
        }
        catch (FormatException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message);
            return RedirectToAction("Error", "home", new { message = 
                "Review's id is wrong" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Critical, ex.Message);
            return RedirectToAction("Error", "home", new { message = 
                "User not found by id" });
        }
    }

    private bool IsReviewValid(ReviewAddModel review)
    {
        if (review.Mark < 0 || review.Mark > 10)
        {
            return false;
        }

        if (review.ImageUrl == null)
        {
            return false;
        }

        return true;
    }

    private async Task<string> UploadImage(IFormFile imgFile)
    {
        var fileName = Path.GetFileNameWithoutExtension(imgFile.FileName);  
        
        var fileExtension = Path.GetExtension(imgFile.FileName);  
        
        fileName = DateTime.Now.ToString("yyyyMMdd")+ "-" + fileName.Trim()+ fileExtension;

        var uploadPath = @"wwwroot\Images";

        var path = uploadPath + "\\" + fileName;
        
        using (Stream fileStream = new FileStream(path, FileMode.Create))
        {
            await imgFile.CopyToAsync(fileStream);
        }
        
        return path.Remove(0, 7).Replace('\\', '/');
    }
}