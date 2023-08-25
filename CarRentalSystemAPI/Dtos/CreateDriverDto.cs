using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateDriverDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The length name must be less than 20 characters.")]
        [MinLength(3, ErrorMessage = "The length name must be greater than 3 characters.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required]
        [Range(18, 80, ErrorMessage = "The Age must be between 18 and 80.")]
        public byte Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The length address must be less than 50 characters.")]
        [MinLength(5, ErrorMessage = "The length address must be greater than 5 characters.")]
        public string Address { get; set; }

        [Required]
        [Range(500, 2000, ErrorMessage = "The Salary must be between 500 and 5000.")]
        public double Salary { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
        
        public Guid? DriverId { get; set; }
    }
}
