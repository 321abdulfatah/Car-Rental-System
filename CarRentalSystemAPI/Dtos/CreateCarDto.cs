using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateCarDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        [Range(1000, 20000, ErrorMessage = "The value must be between 1000 and 20000.")]
        public double EngineCapacity { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public double DailyFare { get; set; }

        public Guid? DriverId { get; set; }
    }
}