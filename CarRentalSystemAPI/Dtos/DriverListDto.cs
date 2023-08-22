using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class DriverListDto : PaginatedResult<DriverDto>
    {
        public string? ErrorMessage { get; set; } = "";

    }
}
