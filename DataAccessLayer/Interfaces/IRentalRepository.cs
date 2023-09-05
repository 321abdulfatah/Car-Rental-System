using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<bool> IsDriverInAnotherRental(Guid driverId);
        Task<bool> IsDriverInAnotherRental(Guid driverId, DateTime startDateRent, DateTime endDateRent);

        Task<bool> IsCarRentedAsync(Guid carId);
        Task<bool> IsCarRentedAsync(Guid carId, DateTime startDateRent, DateTime endDateRent);

    }
}
