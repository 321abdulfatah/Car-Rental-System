using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class DriverRequestDto : PagingModel<DriverDto>
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
    }
}
