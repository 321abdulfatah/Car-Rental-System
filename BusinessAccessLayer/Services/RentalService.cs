using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class RentalService : IRentalService
    {
        public IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> IsDriverAvailableAsync(Guid driverId)
        {
            bool isAvailable = await _unitOfWork.Drivers.IsDriverAvailableAsync(driverId);
            return isAvailable;
        }
        public async Task<bool> CreateRentalAsync(Rental rental)
        {
            if (rental != null)
            {
                if (rental.DriverId != null)
                {
                    var driverId = rental.DriverId;

                    bool isDriverAvailable = await IsDriverAvailableAsync((Guid)driverId);
                    
                    while(!isDriverAvailable && driverId != null)
                    {
                        var driver = await _unitOfWork.Drivers.GetAsync((Guid)driverId);

                        driverId = driver.DriverId;

                        isDriverAvailable = await IsDriverAvailableAsync((Guid)driverId);
                    }

                    rental.DriverId = isDriverAvailable ? driverId : null;
                }

                await _unitOfWork.Rentals.CreateAsync(rental);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteRentalAsync(Guid rentalId)
        {
            var rentalDetails = await _unitOfWork.Rentals.GetAsync(rentalId);
            if (rentalDetails != null)
            {
                _unitOfWork.Rentals.DeleteAsync(rentalId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Rental>> GetAllRentalAsync()
        {
            var includeExpressions = new List<Expression<Func<Rental, object>>>
            {
                r => r.Car,
                r => r.Customer,
                r => r.Driver
            };

            var rentalDetailsList = await _unitOfWork.Rentals.GetAllAsync(includeExpressions);
            return rentalDetailsList;
        }

        public async Task<Rental> GetRentalByIdAsync(Guid rentalId)
        {
            var includeExpressions = new List<Expression<Func<Rental, object>>>
            {
                r => r.Car,
                r => r.Customer,
                r => r.Driver
            };

            var rentalDetails = await _unitOfWork.Rentals.GetAsync(rentalId, includeExpressions);
            if (rentalDetails != null)
            {
                return rentalDetails;
            }
            return null;
        }

        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            if (rental != null)
            {
                var rentalItem = await _unitOfWork.Rentals.GetAsync(rental.Id);
                if (rentalItem != null)
                {
                    _unitOfWork.Rentals.UpdateAsync(rentalItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<PaginatedResult<Rental>> GetListRentalsAsync(Expression<Func<Rental, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Rentals.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}
