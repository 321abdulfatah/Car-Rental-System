namespace CarRentalSystemAPI.Dtos
{
    public class CreateCarDto
    {
        public string Type { get; set; }

        public double EngineCapacity { get; set; }

        public string Color { get; set; }

        public double DailyFare { get; set; }

        public Guid? DriverId { get; set; }
    }
}
