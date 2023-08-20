using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<bool> IsCarRentedAsync(Guid carId);

        Task<PaginatedResult<Rental>> GetListAsync(Expression<Func<Rental, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);


    }
}
