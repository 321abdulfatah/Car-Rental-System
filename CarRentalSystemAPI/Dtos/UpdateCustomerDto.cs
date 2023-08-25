using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateCustomerDto : CreateCustomerDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
