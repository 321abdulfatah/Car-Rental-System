using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class DriverRequestDto : PagingModel<DriverDto>
    {
        public string? Search { get; set; }
        public string? Column { get; set; }
        public string? SortOrder { get; set; }
        public string? OrderBy { get; set; }
    }
}
