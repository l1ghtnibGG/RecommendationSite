using System.ComponentModel.DataAnnotations;

namespace RecommendationSite.Models
{
    public class LogIn
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
