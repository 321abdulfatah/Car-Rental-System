using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly CarRentalDBContext _dbContext;

        public CarRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<double> GetDailyFare(Guid carId)
        {
            var car = await _dbContext.Cars.FindAsync(carId);
            return car.DailyFare;
        }

    }
}
