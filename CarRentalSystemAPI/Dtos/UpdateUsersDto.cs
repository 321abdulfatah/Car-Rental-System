using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class UpdateUsersDto : CreateUsersDto
    {
        public Guid Id { get; set; }
    }
}
