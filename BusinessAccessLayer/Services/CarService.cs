using Abp.Domain.Entities;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Abp.Collections.Extensions;

namespace BusinessAccessLayer.Services
    {
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMemoryCache _memoryCache;

        public CarService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<bool> CreateCarAsync(Car car)
        {
            if (car != null)
            {
                await _unitOfWork.Cars.CreateAsync(car);

                var result = _unitOfWork.Save();

                if (result > 0)
                {
                    var carInMemoryCache = _memoryCache.Get(CacheKeys.Cars) as List<Car>;
                    
                    carInMemoryCache.Add(car);

                    _memoryCache.Remove(CacheKeys.Cars);
                    
                    _memoryCache.Set(CacheKeys.Cars, carInMemoryCache);

                    return true;
                }
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
                    await _unitOfWork.Cars.DeleteAsync(carId);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                    {
                        var carInMemoryCache = _memoryCache.Get(CacheKeys.Cars) as List<Car>;

                        var oldCarRecord = carInMemoryCache.FirstOrDefault(c => c.Id == carId);
                        carInMemoryCache.Remove(oldCarRecord);

                        _memoryCache.Remove(CacheKeys.Cars);

                        _memoryCache.Set(CacheKeys.Cars, carInMemoryCache);

                        return true;
                    }
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
            /*var includeExpressions = new List<Expression<Func<Car, object>>>
            {
                c => c.Driver,
            };*/
            var carInMemoryCache = _memoryCache.Get(CacheKeys.Cars) as List<Car>;

            var carDetails = carInMemoryCache.FirstOrDefault(c => c.Id == carId);

            //var carDetails = await _unitOfWork.Cars.GetAsync(carId, includeExpressions);

            if (carDetails == null)
            {
                throw new EntityNotFoundException($"Car with ID {carId} not found.");
            }
            return carDetails;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            if (car!= null)
            {
                var carEntity = await _unitOfWork.Cars.GetAsync(car.Id);
                
                if (carEntity == null)
                    throw new EntityNotFoundException($"Car with ID {car.Id} not found.");
                

                await _unitOfWork.Cars.UpdateAsync(car);

                var result = _unitOfWork.Save();

                if (result > 0)
                {
                    var carInMemoryCache = _memoryCache.Get(CacheKeys.Cars) as List<Car>;

                    var oldCarRecord = carInMemoryCache.FirstOrDefault(c => c.Id == car.Id);
                    var oldCarRecordIndex = carInMemoryCache.FindIndex(c => c.Id == car.Id);
                    
                    carInMemoryCache.Remove(oldCarRecord);
                    carInMemoryCache.Insert(oldCarRecordIndex,car);

                    _memoryCache.Remove(CacheKeys.Cars);

                    _memoryCache.Set(CacheKeys.Cars, carInMemoryCache);

                    return true;
                }
                else
                    return false;
                
            }
            return false;
        }

        public async Task<PaginatedResult<Car>> GetListCarsAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {

            var carInMemoryCache = _memoryCache.Get(CacheKeys.Cars) as IEnumerable<Car>;

            //Expression<Func<Car, bool>> filter = car => true;
            var query = carInMemoryCache;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Apply filter
                query = carInMemoryCache.Where(car => car.Color.Contains(searchTerm) || car.Type.Contains(searchTerm) ||
                                           car.EngineCapacity.ToString().Equals(searchTerm) ||  
                                           car.DailyFare.ToString().Equals(searchTerm));
            }
            
            

            // Calculate total count
            var totalCount = query.Count();

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

            var pagedCars = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var result = new PaginatedResult<Car>
            {
                Data = pagedCars,
                TotalCount = totalCount
            };

            return result;
        }
    }
}
