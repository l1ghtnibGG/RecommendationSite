using System.ComponentModel.DataAnnotations;

namespace RecommendationSite.Models
{
    public class UserRegistration
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password didn't match")]
        public string ConfirmPassword { get; set; }
    }
}
