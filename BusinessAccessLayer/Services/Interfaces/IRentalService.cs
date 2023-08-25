using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IRentalService
    {
        Task<bool> IsDriverAvailableAsync(Guid driverId);

        Task<bool> CreateRentalAsync(Rental rental);

        Task<Rental> GetRentalByIdAsync(Guid rentalId);

        Task<bool> UpdateRentalAsync(Rental rental);

        Task<bool> DeleteRentalAsync(Guid rentalId);
        
        Task<PaginatedResult<Rental>> GetListRentalsAsync(string searchTerm, string sortBy, int pageIndex, int pageSize);

    }
}
