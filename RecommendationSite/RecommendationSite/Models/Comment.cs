using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecommendationSite.Models;

public class Comment
{
    [Required] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public string Text { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime CreateDate { get; set; }
    
    public Guid? ReviewId { get; set; }
    public Review Review { get; set; }
    
    public Guid? UserId { get; set; }
    public User User { get; set; }

}