using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<double> GetDailyFare(Guid carId);

    }
}
