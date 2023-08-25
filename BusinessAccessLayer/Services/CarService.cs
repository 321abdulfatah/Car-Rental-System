using Abp.Domain.Entities;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Car> GetCarByIdAsync(Guid carId)
        {
            var includeExpressions = new List<Expression<Func<Car, object>>>
            {
                c => c.Driver,
            };
            var carDetails = await _unitOfWork.Cars.GetAsync(carId, includeExpressions);

            if (carDetails == null)
            {
                //throw new Exceptions.EntityNotFoundException($"Couldn't find the Car with Id {carId} in the Database.");

                throw new EntityNotFoundException($"Car with ID {carId} not found.");
            }
            return carDetails;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            if (car!= null)
            {
                
                await _unitOfWork.Cars.UpdateAsync(car);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
                
            }
            return false;
        }

        public async Task<PaginatedResult<Car>> GetListCarsAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {

            var includeExpressions = new List<Expression<Func<Car, object>>>
            {
                c => c.Driver,
            };
            Expression<Func<Car, bool>> filter = car => true;
            
            if (!string.IsNullOrEmpty(searchTerm))
                filter = car => car.Color.Contains(searchTerm) || car.Type.Contains(searchTerm) ||
                                         car.EngineCapacity.ToString().Equals(searchTerm) ||
                                         car.DailyFare.ToString().Equals(searchTerm);

            var query = _unitOfWork.Cars.GetAll(includeExpressions);

            // Apply filter
            query = query.Where(filter);

            // Calculate total count
            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                // Split the sortBy string into column name and sorting direction
                var sortByParts = sortBy.Split(' ');
                string columnName = sortByParts[0];
                string sortingDirection = sortByParts.Length > 1 ? sortByParts[1] : "asc"; // Default to ascending

                // Determine the sorting order
                bool isDescending = sortingDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
                
                switch (columnName.ToLower())
                {
                    case "color":
                        query = isDescending ? query.OrderByDescending(c => c.Color) : query.OrderBy(c => c.Color);
                        break;
                    case "dailyfare":
                        query = isDescending ? query.OrderByDescending(c => c.DailyFare) : query.OrderBy(c => c.DailyFare);
                        break;
                    case "enginecapacity":
                        query = isDescending ? query.OrderByDescending(c => c.EngineCapacity) : query.OrderBy(c => c.EngineCapacity);
                        break;
                    case "type":
                        query = isDescending ? query.OrderByDescending(c => c.Type) : query.OrderBy(c => c.Type);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }

            var pagedCars = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Car>
            {
                Data = pagedCars,
                TotalCount = totalCount
            };
        }
    }
}
