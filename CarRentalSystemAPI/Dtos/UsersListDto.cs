using DataAccessLayer.Common.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UsersListDto : PaginatedResult<UsersDto>
    {
    }
}
