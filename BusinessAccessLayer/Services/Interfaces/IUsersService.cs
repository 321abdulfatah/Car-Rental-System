using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IUsersService
    {
        Task<bool> CreateUsersAsync(Users users);

        Task<IEnumerable<Users>> GetAllUsersAsync();

        Task<Users> GetUsersByIdAsync(Guid usersId);

        Task<bool> UpdateUsersAsync(Users users);

        Task<bool> DeleteUsersAsync(Guid usersId);
        Task<PaginatedResult<Users>> GetListUsersAsync(Expression<Func<Users, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
