using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

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

        public async Task<bool> DeleteCar(Guid carId)
        {
            var carDetails = await _unitOfWork.Cars.Get(carId);
            if (carDetails != null)
            {
                _unitOfWork.Cars.Delete(carDetails);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
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
    }
}
