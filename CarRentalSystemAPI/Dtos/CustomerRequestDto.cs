using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CustomerRequestDto : PagingModel<CustomerDto>
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
    }
}
