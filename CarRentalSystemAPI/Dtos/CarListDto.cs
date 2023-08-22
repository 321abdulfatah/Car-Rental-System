using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CarListDto : PaginatedResult<CarDto>
    {
        public string? ErrorMessage { get; set; } = "";

    }
}
