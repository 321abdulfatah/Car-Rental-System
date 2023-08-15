namespace DataAccessLayer.Models
{
    public class Driver:Person
    {

        public Guid? DriverId { get; set; }

        public Driver BehalfOfDriver { get; set; }

        public double Salary { get; set; }

        public bool IsAvailable { get; set; }
    }
}
