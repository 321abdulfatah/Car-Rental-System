using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class RentalRequestDto : PagingModel<RentalDto>
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
    }
}
