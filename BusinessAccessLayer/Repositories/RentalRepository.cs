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

    }
}
