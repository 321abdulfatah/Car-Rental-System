using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class CarService : ICarService
    {
        public IUnitOfWork _unitOfWork;

        public CarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateCarAsync(Car car)
        {
            if (car != null)
            {
                await _unitOfWork.Cars.CreateAsync(car);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> CanDeleteCarAsync(Guid carId)
        {
            bool isRented = await _unitOfWork.Rentals.IsCarRentedAsync(carId);
            return !isRented;
        }
        public async Task<bool> DeleteCarAsync(Guid carId)
        {
            var carDetails = await _unitOfWork.Cars.GetAsync(carId);
            
            if (carDetails != null)
            {
                bool canDelete = await CanDeleteCarAsync(carId);

                if (canDelete)
                {
                    _unitOfWork.Cars.DeleteAsync(carId);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    throw new Exception("Car cannot be deleted because it is rented.");
                }
            }
            return false;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {

            var carDetailsList = await _unitOfWork.Cars.GetAllAsync();
            return carDetailsList;
        }

        public async Task<Car> GetCarByIdAsync(Guid carId)
        {
            var carDetails = await _unitOfWork.Cars.GetAsync(carId);
            if (carDetails != null)
            {
                return carDetails;
            }
            return null;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            if (car!= null)
            {
                var carItem = await _unitOfWork.Cars.GetAsync(car.Id);
                if (carItem != null)
                {
                    _unitOfWork.Cars.UpdateAsync(carItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<PaginatedResult<Car>> GetListCarsAsync(Expression<Func<Car, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Cars.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}
