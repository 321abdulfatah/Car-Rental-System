using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CarRequestDto : PagingModel<CarDto>
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
    }
}
