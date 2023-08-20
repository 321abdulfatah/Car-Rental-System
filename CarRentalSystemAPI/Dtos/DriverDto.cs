namespace CarRentalSystemAPI.Dtos
{
    public class DriverDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Salary { get; set; }
        public bool IsAvailable { get; set; }
        public string ErrorMessage { get; set; } = "";


    }
}
