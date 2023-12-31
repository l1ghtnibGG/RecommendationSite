﻿using System.ComponentModel.DataAnnotations;

namespace RecommendationSite.Models
{
    public class UserLogIn
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
