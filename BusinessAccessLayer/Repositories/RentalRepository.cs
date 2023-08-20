using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        private readonly CarRentalDBContext _dbContext;

        public RentalRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsCarRentedAsync(Guid carId)
        {
            return await _dbContext.Rentals.AnyAsync(r => r.CarId == carId);
        }

        public async Task<PaginatedResult<Rental>> GetListAsync(Expression<Func<Rental, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Rentals.Where(filter);

            switch (sortBy)
            {
                case "rentTerm":
                    query = isAscending ? query.OrderBy(c => c.RentTerm) : query.OrderByDescending(c => c.RentTerm);
                    break;
                case "age":
                    query = isAscending ? query.OrderBy(c => c.Rent) : query.OrderByDescending(c => c.Rent);
                    break;
                case "startDateRent":
                    query = isAscending ? query.OrderBy(c => c.StartDateRent) : query.OrderByDescending(c => c.StartDateRent);
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var pagedRentals = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Rental>
            {
                Data = pagedRentals,
                TotalCount = await query.CountAsync()
            };
        }
    }
}
