using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IUsersRepository : IRepository<Users>
    {
        Task<PaginatedResult<Users>> GetListAsync(Expression<Func<Users, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
