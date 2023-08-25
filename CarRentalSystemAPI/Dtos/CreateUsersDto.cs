using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateUsersDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The length name must be less than 20 characters.")]
        [MinLength(3, ErrorMessage = "The length name must be greater than 3 characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The length password must be less than 20 characters.")]
        [MinLength(4, ErrorMessage = "The length password must be greater than 4 characters.")]
        public string Password { get; set; }
    }
}
