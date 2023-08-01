using DataAccessLayer.Enums;

namespace DataAccessLayer.Models
{
    public class Rental : BaseModel
    {
        public Guid CarId { get; set; }

        public virtual Car Car { get; set; }
        
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public Guid? DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public int Rent { get; set; }

        public StatusRent StatusRent { get; set; }

        public DateTime StratDateRent{ get; set; }

        public int RentTerm { get; set; }
    }
}