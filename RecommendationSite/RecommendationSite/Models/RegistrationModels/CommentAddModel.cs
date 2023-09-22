using System.ComponentModel.DataAnnotations;

namespace RecommendationSite.Models.RegistrationModels;

public class CommentAddModel
{
    public string Title { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string Text { get; set; }

    public Guid? ReviewId { get; set; }

    public Guid? UserId { get; set; }
}