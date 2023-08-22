using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateCarDto : CreateCarDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
