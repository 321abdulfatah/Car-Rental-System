using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICarService
    {
        Task<bool> CreateCarAsync(Car car);

        Task<IEnumerable<Car>> GetAllCarsAsync();

        Task<Car> GetCarByIdAsync(Guid carId);

        Task<bool> UpdateCarAsync(Car car);

        Task<bool> CanDeleteCarAsync(Guid carId);

        Task<bool> DeleteCarAsync(Guid carId);
        
        Task<PaginatedResult<Car>> GetListCarsAsync(string searchTerm, string sortBy, int pageIndex, int pageSize);
    }
}
