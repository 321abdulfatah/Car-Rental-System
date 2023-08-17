using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    public class DriverService : IDriverService
    {
        public IUnitOfWork _unitOfWork;

        public DriverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateDriver(Driver driver)
        {
            if (driver != null)
            {
                await _unitOfWork.Drivers.Create(driver);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteDriver(Guid driverId)
        {
            var driverDetails = await _unitOfWork.Drivers.Get(driverId);
            if (driverDetails != null)
            {
                _unitOfWork.Drivers.Delete(driverDetails);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Driver>> GetAllDriver()
        {
            var driverDetailsList = await _unitOfWork.Drivers.GetList();
            return driverDetailsList;
        }

        public async Task<Driver> GetDriverById(Guid driverId)
        {
            var driverDetails = await _unitOfWork.Drivers.Get(driverId);
            if (driverDetails != null)
            {
                return driverDetails;
            }
            return null;
        }

        public async Task<bool> UpdateDriver(Customer driver)
        {
            if (driver != null)
            {
                var driverItem = await _unitOfWork.Drivers.Get(driver.Id);
                if (driverItem != null)
                {
                    _unitOfWork.Drivers.Update(driverItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
