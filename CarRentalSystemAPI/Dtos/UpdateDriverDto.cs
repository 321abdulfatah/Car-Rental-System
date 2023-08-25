using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateDriverDto : CreateDriverDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
