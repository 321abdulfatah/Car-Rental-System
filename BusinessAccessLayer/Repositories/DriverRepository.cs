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
        
    }
}