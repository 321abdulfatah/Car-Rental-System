using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccessLayer.Repositories
{
    public class DriverRepository : Repository<Driver>, IDriverRepository
    {
        private readonly CarRentalDBContext _dbContext;
        public DriverRepository(CarRentalDBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<bool> IsDriverAvailableAsync(Guid driverId)
        {
            var driver = await _dbContext.Drivers.FindAsync(driverId);
            return driver != null && driver.IsAvailable;
        }
        public async Task<PaginatedResult<Driver>> GetListAsync(Expression<Func<Driver, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Drivers.Where(filter);

            switch (sortBy)
            {
                case "name":
                    query = isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "age":
                    query = isAscending ? query.OrderBy(c => c.Age) : query.OrderByDescending(c => c.Age);
                    break;
                case "salary":
                    query = isAscending ? query.OrderBy(c => c.Salary) : query.OrderByDescending(c => c.Salary);
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var pagedDrivers = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Driver>
            {
                Data = pagedDrivers,
                TotalCount = await query.CountAsync()
            };
        }
    }
}