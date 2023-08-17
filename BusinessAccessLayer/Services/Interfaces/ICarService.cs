using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICarService
    {
        Task<bool> CreateCar(Car car);

        Task<IEnumerable<Car>> GetAllCars();

        Task<Car> GetCarById(Guid carId);

        Task<bool> UpdateCar(Car car);

        Task<bool> DeleteCar(Guid carId);
    }
}
