using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

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
        
        public Guid? UserId { get; set; }
        public User User { get; set; }
        
        public List<Score> Scores { get; set; }
        public List<Tag> Tags { get; set; } = new();
        
        public List<Comment> Comments { get; set; } = new();
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum GroupType 
        {
            Book = 1,
            Movie,
            Game
        }
    }
}
