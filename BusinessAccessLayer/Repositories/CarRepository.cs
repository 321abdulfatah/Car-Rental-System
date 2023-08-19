using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Repositories
{
    public class CarRepository : Repository < Car > , ICarRepository
    {
        private readonly CarRentalDBContext _dbContext;

        public CarRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

      

        public async Task<PaginatedResult<Car>> GetSortedFilteredCarsAsync(Expression<Func<Car, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Cars.Where(filter);

            switch (sortBy)
            {
                case "color":
                    query = isAscending ? query.OrderBy(c => c.Color) : query.OrderByDescending(c => c.Color);
                    break;
                case "dailyFare":
                    query = isAscending ? query.OrderBy(c => c.DailyFare) : query.OrderByDescending(c => c.DailyFare);
                    break;
                case "engineCapacity":
                    query = isAscending ? query.OrderBy(c => c.EngineCapacity) : query.OrderByDescending(c => c.EngineCapacity);
                    break;
                case "type":
                    query = isAscending ? query.OrderBy(c => c.Type) : query.OrderByDescending(c => c.Type);
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var pagedCars = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Car>
            {
                Data = pagedCars,
                TotalCount = await query.CountAsync()
            };
        }
    }
}
