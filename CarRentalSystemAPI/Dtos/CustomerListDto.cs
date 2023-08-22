using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CustomerListDto : PaginatedResult<CustomerDto>
    {
        public string? ErrorMessage { get; set; } = "";

    }
}
