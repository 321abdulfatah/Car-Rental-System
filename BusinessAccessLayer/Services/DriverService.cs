using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class DriverService : IDriverService
    {
        public IUnitOfWork _unitOfWork;

        public DriverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateDriverAsync(Driver driver)
        {
            if (driver != null)
            {
                await _unitOfWork.Drivers.CreateAsync(driver);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteDriverAsync(Guid driverId)
        {
            var driverDetails = await _unitOfWork.Drivers.GetAsync(driverId);
            if (driverDetails != null)
            {
                _unitOfWork.Drivers.DeleteAsync(driverId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Driver>> GetAllDriverAsync()
        {
            var driverDetailsList = await _unitOfWork.Drivers.GetAllAsync();
            return driverDetailsList;
        }

        public async Task<Driver> GetDriverByIdAsync(Guid driverId)
        {
            var driverDetails = await _unitOfWork.Drivers.GetAsync(driverId);
            if (driverDetails != null)
            {
                return driverDetails;
            }
            return null;
        }

        public async Task<bool> UpdateDriverAsync(Driver driver)
        {
            if (driver != null)
            {
                var driverItem = await _unitOfWork.Drivers.GetAsync(driver.Id);
                if (driverItem != null)
                {
                    _unitOfWork.Drivers.UpdateAsync(driverItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<PaginatedResult<Driver>> GetListDriversAsync(Expression<Func<Driver, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Drivers.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}
