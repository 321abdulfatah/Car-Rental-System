using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IDriverService
    {
        Task<bool> CreateDriver(Driver driver);

        Task<IEnumerable<Driver>> GetAllDriver();

        Task<Driver> GetDriverById(Guid driverId);

        Task<bool> UpdateDriver(Customer driver);

        Task<bool> DeleteDriver(Guid driverId);
    }
}
