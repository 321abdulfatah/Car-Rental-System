using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateCarDto : CreateCarDto
    {
        public Guid Id { get; set; }
    }
}
