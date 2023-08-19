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

        public async Task<bool> CreateCar(Car car)
        {
            if (car != null)
            {
                await _unitOfWork.Cars.Create(car);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> CanDeleteCar(Guid carId)
        {
            bool isRented = await _unitOfWork.Rentals.IsCarRentedAsync(carId);
            return !isRented;
        }
        public async Task<bool> DeleteCar(Guid carId)
        {
            var carDetails = await _unitOfWork.Cars.Get(carId);
            
            if (carDetails != null)
            {
                bool canDelete = await CanDeleteCar(carId);

                if (canDelete)
                {
                    _unitOfWork.Cars.Delete(carId);
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

        public async Task<IEnumerable<Car>> GetAllCars()
        {

            var carDetailsList = await _unitOfWork.Cars.GetList();
            return carDetailsList;
        }

        public async Task<Car> GetCarById(Guid carId)
        {
            var carDetails = await _unitOfWork.Cars.Get(carId);
            if (carDetails != null)
            {
                return carDetails;
            }
            return null;
        }

        public async Task<bool> UpdateCar(Car car)
        {
            if (car!= null)
            {
                var carItem = await _unitOfWork.Cars.Get(car.Id);
                if (carItem != null)
                {
                    _unitOfWork.Cars.Update(carItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<PaginatedResult<Car>> GetFilteredAndSortedCars(Expression<Func<Car, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Cars.GetSortedFilteredCarsAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }


    }
}
