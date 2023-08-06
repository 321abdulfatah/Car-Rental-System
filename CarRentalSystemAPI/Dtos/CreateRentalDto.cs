using DataAccessLayer.Enums;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateRentalDto
    {
        public Guid CarId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? DriverId { get; set; }

        public int Rent { get; set; }

        public StatusRent StatusRent { get; set; }

        public DateTime StartDateRent { get; set; }

        public int RentTerm { get; set; }
    }
}
