using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<PaginatedResult<Car>> GetListAsync(Expression<Func<Car, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
