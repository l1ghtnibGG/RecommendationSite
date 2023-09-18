using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecommendationSite.Models;

public class Score
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public short UserScore { get; set; }

    public LikeStatus Like { get; set; } = LikeStatus.Neutral;

    public Guid? UserId { get; set; }
    public User User { get; set; }
    
    public Guid? ReviewId { get; set; }
    public Review Review { get; set; }

    public enum LikeStatus
    {
        Like, 
        Dislike,
        Neutral
    }
}