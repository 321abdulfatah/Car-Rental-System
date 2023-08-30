﻿using Abp.Domain.Entities;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessAccessLayer.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMemoryCache _memoryCache;


        public RentalService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
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
                if (rental.StartDateRent <= DateTime.Now)
                {
                    throw new Exception("The car cannot be rented with a previous date, " +
                                        $"the date must be after the current date and time {DateTime.Now}");
                }

                if (rental.DriverId.HasValue && rental.DriverId != Guid.Empty)
                {
                    var driverId = rental.DriverId;

                    bool isDriverAvailable = await IsDriverAvailableAsync((Guid)driverId);
                    
                    while(!isDriverAvailable && driverId.HasValue && driverId != Guid.Empty)
                    {
                        var driver = await _unitOfWork.Drivers.GetAsync((Guid)driverId);

                        driverId = driver.ReplacmentDriverId;

                        isDriverAvailable = await IsDriverAvailableAsync((Guid)driverId);
                    }

                    rental.DriverId = isDriverAvailable ? driverId : 
                        throw new Exception("The current driver for this car is currently unavailable " +
                                            "and there is no other driver to replace him");
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
                await _unitOfWork.Rentals.DeleteAsync(rentalId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
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
            if (rentalDetails == null)
            {
                throw new EntityNotFoundException($"Rental with ID {rentalId} not found.");
                
            }
            return rentalDetails;
        }

        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            if (rental != null)
            {
                var rentalEntity = await _unitOfWork.Rentals.GetAsync(rental.Id);

                if (rentalEntity == null)
                    throw new EntityNotFoundException($"Rental with ID {rental.Id} not found.");

                await _unitOfWork.Rentals.UpdateAsync(rental);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
                
            }
            return false;
        }
        public async Task<PaginatedResult<Rental>> GetListRentalsAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {
            var cacheKey = $"{searchTerm}_{sortBy}_{pageIndex}_{pageSize}";
            
            if (_memoryCache.TryGetValue(cacheKey, out PaginatedResult<Rental> cachedResult))
            {
                return cachedResult;
            }

            var includeExpressions = new List<Expression<Func<Rental, object>>>
            {
                r => r.Car,
                r => r.Customer,
                r => r.Driver
            };

            Expression<Func<Rental, bool>> filter = rental => true;

            if (!string.IsNullOrEmpty(searchTerm))
                filter = rental => rental.Rent.ToString().Equals(searchTerm) || 
                                   rental.RentTerm.ToString().Equals(searchTerm) ||
                                   rental.StatusRent.ToString().Equals(searchTerm) ||
                                   rental.StartDateRent.ToString().Contains(searchTerm);

            var query = _unitOfWork.Rentals.GetAll(includeExpressions);

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
                    case "rent":
                        query = isDescending ? query.OrderByDescending(c => c.Rent) : query.OrderBy(c => c.Rent);
                        break;
                    case "rentterm":
                        query = isDescending ? query.OrderByDescending(c => c.RentTerm) : query.OrderBy(c => c.RentTerm);
                        break;
                    case "startdaterent":
                        query = isDescending ? query.OrderByDescending(c => c.StartDateRent) : query.OrderBy(c => c.StartDateRent);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }

            var pagedRentals = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PaginatedResult<Rental>
            {
                Data = pagedRentals,
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
