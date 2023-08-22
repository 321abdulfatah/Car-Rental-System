using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalSystemAPI.Dtos
{
    public class CarDto
    {
        public Guid Id { get; set; }
        
        public string Type { get; set; }
        
        public double EngineCapacity { get; set; }
        
        public string Color { get; set; }
        
        public double DailyFare { get; set; }
        
        public Guid? DriverId { get; set; }
        
        public DriverDto Driver { get; set; }
    }
}
