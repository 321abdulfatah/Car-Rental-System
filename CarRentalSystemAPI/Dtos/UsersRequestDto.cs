using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class UsersRequestDto : PagingModel<UsersDto>
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
    }
}
