using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateRentalDto : CreateRentalDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
