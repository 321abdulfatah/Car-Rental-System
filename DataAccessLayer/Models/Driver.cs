namespace DataAccessLayer.Models
{
    public class Driver:Person
    {

        public Guid? ReplacmentDriverId { get; set; }

        public Driver ReplacmentDriver { get; set; }

        public double Salary { get; set; }

        public bool IsAvailable { get; set; }
    }
}
