using System.ComponentModel.DataAnnotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateUsersDto : CreateUsersDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
