using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace RecommendationSite.Models.RegistrationModels;

public class ReviewAddModel
{
    public Guid Id { get; set; }
    
    [Required] 
    public string Name { get; set; }

    [Required] 
    public string Title { get; set; }

    public Review.GroupType Group { get; set; }

    [DataType(DataType.Text)] 
    public string Text { get; set; }
    
    public IFormFile ImageUrl { get; set; }
    
    public short Mark { get; set; }
    
    public Guid UserId { get; set; }
    
    public string IsAdd { get; set; }
}