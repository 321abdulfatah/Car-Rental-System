using BusinessAccessLayer.Exceptions;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessAccessLayer.Services
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMemoryCache _memoryCache;


        public DriverService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
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
                await _unitOfWork.Drivers.DeleteAsync(driverId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<Driver> GetDriverByIdAsync(Guid driverId)
        {
            var driverDetails = await _unitOfWork.Drivers.GetAsync(driverId);
            if (driverDetails == null)
            {
                throw new NotFoundException($"Driver with ID {driverId} not found.");
            }
            return driverDetails;
        }

        public async Task<bool> UpdateDriverAsync(Driver driver)
        {
            if (driver != null)
            {
                var driverEntity = await _unitOfWork.Drivers.GetAsync(driver.Id);

                if (driverEntity == null)
                    throw new NotFoundException($"Driver with ID {driver.Id} not found.");

                await _unitOfWork.Drivers.UpdateAsync(driver);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            
            return false;
        }
        public async Task<PaginatedResult<Driver>> GetListDriversAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {
            var cacheKey = $"{searchTerm}_{sortBy}_{pageIndex}_{pageSize}";

            if (_memoryCache.TryGetValue(cacheKey, out PaginatedResult<Driver> cachedResult))
            {
                return cachedResult;
            }

            Expression<Func<Driver, bool>> filter = driver => true;

            if (!string.IsNullOrEmpty(searchTerm))
                filter = driver =>   driver.Address.Contains(searchTerm) ||
                                     driver.Name.Contains(searchTerm) ||
                                     driver.Phone.ToString().Equals(searchTerm) ||
                                     driver.Age.ToString().Equals(searchTerm) ||
                                     driver.Email.Equals(searchTerm) ||
                                     driver.Gender.Equals(searchTerm);

            var query = _unitOfWork.Drivers.GetAll();

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
                    case "address":
                        query = isDescending ? query.OrderByDescending(c => c.Address) : query.OrderBy(c => c.Address);
                        break;
                    case "name":
                        query = isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                        break;
                    case "age":
                        query = isDescending ? query.OrderByDescending(c => c.Age) : query.OrderBy(c => c.Age);
                        break;
                    case "phone":
                        query = isDescending ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }

            var pagedDrivers = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PaginatedResult<Driver>
            {
                Data = pagedDrivers,
                TotalCount = totalCount
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(45))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
            .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }
    }
}
