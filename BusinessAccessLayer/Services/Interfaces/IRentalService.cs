using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IRentalService
    {
        Task<bool> CreateRentalAsync(Rental rental);

        Task<IEnumerable<Rental>> GetAllRentalAsync();

        Task<Rental> GetRentalByIdAsync(Guid rentalId);

        Task<bool> UpdateRentalAsync(Rental rental);

        Task<bool> DeleteRentalAsync(Guid rentalId);
        Task<PaginatedResult<Rental>> GetListRentalsAsync(Expression<Func<Rental, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
