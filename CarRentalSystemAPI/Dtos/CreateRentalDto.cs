using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateRentalDto
    {
        [Required]
        public Guid CarId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? DriverId { get; set; }

        [Required]
        public DateTime StartDateRent { get; set; }

        [Required]
        [Range(1, 365, ErrorMessage = "The value must be between 1 and 365 days.")]
        public int RentTerm { get; set; }
    }
}
