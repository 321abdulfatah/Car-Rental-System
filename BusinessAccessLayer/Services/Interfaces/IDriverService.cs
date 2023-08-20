using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IDriverService
    {
        Task<bool> CreateDriverAsync(Driver driver);

        Task<IEnumerable<Driver>> GetAllDriverAsync();

        Task<Driver> GetDriverByIdAsync(Guid driverId);

        Task<bool> UpdateDriverAsync(Driver driver);

        Task<bool> DeleteDriverAsync(Guid driverId);
        Task<PaginatedResult<Driver>> GetListDriversAsync(Expression<Func<Driver, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
