using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICarService
    {
        Task<bool> CreateCar(Car car);

        Task<IEnumerable<Car>> GetAllCars();

        Task<Car> GetCarById(Guid carId);

        Task<bool> UpdateCar(Car car);

        Task<bool> CanDeleteCar(Guid carId);

        Task<bool> DeleteCar(Guid carId);
        
        Task<PaginatedResult<Car>> GetFilteredAndSortedCars(Expression<Func<Car, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);
    }
}
