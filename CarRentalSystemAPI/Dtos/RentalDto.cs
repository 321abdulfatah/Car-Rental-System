using DataAccessLayer.Models;
using DataAccessLayer.Enums;

namespace CarRentalSystemAPI.Dtos
{
    public class RentalDto
    {
        public Guid Id { get; set; }

        public Guid CarId { get; set; }

        public virtual Car Car { get; set; }

        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public Guid? DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public int Rent { get; set; }

        public StatusRent StatusRent { get; set; }

        public DateTime StartDateRent { get; set; }

        public int RentTerm { get; set; }
    }
}
