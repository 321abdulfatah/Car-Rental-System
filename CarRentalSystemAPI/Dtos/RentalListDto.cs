using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class RentalListDto : PaginatedResult<RentalDto>
    {
        public string? ErrorMessage { get; set; } = "";

    }
}
