using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IDriverRepository : IRepository<Driver>
    {
        Task<bool> IsDriverAvailableAsync(Guid driverId);

        Task<PaginatedResult<Driver>> GetListAsync(Expression<Func<Driver, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
