using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecommendationSite.Models
{
    public class User
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.User;

        public DateTime CreatedDate { get; set; }

        public DateTime LastLogin { get; set; }
        
        public List<Score> Scores { get; set; } = new();
        
        public List<Review> Reviews { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public enum StatusType
        {
            User,
            Admin
        }
    }
}
