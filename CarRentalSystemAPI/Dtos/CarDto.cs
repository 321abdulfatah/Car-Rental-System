using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CarDto : BaseModel
    {
        public Guid? DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public string Type { get; set; }

        public double EngineCapacity { get; set; }

        public string Color { get; set; }

        public double DailyFare { get; set; }
    }
}
