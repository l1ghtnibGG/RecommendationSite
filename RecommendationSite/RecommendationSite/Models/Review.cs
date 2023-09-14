using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace RecommendationSite.Models
{
    public class Review
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }   

        public GroupType Group { get; set; }   
        
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public short Mark { get; set; }
        
        public Score Score { get; set; }

        public List<Tag> Tags { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public Guid UserId { get; set; }
        public User User { get; set; }
        

        public enum GroupType 
        {
            Book,
            Movie,
            Game
        }
    }
}
