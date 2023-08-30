using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface IDriverRepository : IRepository<Driver>
    {
        Task<bool> IsDriverAvailableAsync(Guid driverId);

    }
}
