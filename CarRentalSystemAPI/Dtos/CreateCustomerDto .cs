using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}
