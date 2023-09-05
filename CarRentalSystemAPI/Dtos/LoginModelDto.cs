﻿using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
