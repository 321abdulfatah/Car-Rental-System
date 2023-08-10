using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class UsersRequestDto : PagingModel<UsersDto>
    {
        public string? Search { get; set; }
        public string? Column { get; set; }
        public string? SortOrder { get; set; }
        public string? OrderBy { get; set; }
    }
}
