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
        public int Rent { get; set; }

        [Required]
        public StatusRent StatusRent { get; set; }

        [Required]
        public DateTime StartDateRent { get; set; }

        [Required]
        public int RentTerm { get; set; }
    }
}
