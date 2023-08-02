namespace DataAccessLayer.Models
{
    public class Car : BaseModel
    {
        public string Type   { get; set; }
        
        public double EngineCapacity   { get; set; }
        
        public string Color { get; set; }

        public double DailyFare { get; set; }

        public Guid? DriverId { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
