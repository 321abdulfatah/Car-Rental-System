using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<bool> IsCarRentedAsync(Guid carId);

    }
}
