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

        public async Task<bool> IsDriverInAnotherRental(Guid driverId)
        {
            return await _dbContext.Rentals.AnyAsync(r => r.DriverId == driverId);
        }
        public async Task<bool> IsDriverInAnotherRental(Guid driverId, DateTime startDateRent, DateTime endDateRent)
        {
            bool isDriverInAnotherRental = false;
            
            var rentedCarList = await _dbContext.Rentals.Where(r => r.DriverId == driverId).ToListAsync();

            if (rentedCarList.Count != 0)
            {
                foreach (var rentedCar in rentedCarList)
                {
                    DateTime beginningRent = rentedCar.StartDateRent;
                    DateTime endRent = rentedCar.StartDateRent.AddDays(rentedCar.RentTerm);

                    if ((endRent < startDateRent) || (beginningRent > endDateRent))
                        isDriverInAnotherRental = false;
                    else
                    {
                        isDriverInAnotherRental = true;
                        break;
                    }
                }

            }
            return isDriverInAnotherRental;
        }
        public async Task<bool> IsCarRentedAsync(Guid carId)
        {
            return await _dbContext.Rentals.AnyAsync(r => r.CarId == carId);
        }
        public async Task<bool> IsCarRentedAsync(Guid carId, DateTime startDateRent, DateTime endDateRent)
        {
            bool isRented = false;
            var rentedCarList = await _dbContext.Rentals.Where(r => r.CarId == carId).ToListAsync();
            
            if (rentedCarList.Count != 0)
            {
                foreach (var rentedCar in rentedCarList)
                {
                    DateTime beginningRent = rentedCar.StartDateRent;
                    DateTime endRent = rentedCar.StartDateRent.AddDays(rentedCar.RentTerm);

                    if ((endRent < startDateRent) || (beginningRent > endDateRent))
                        isRented = false;
                    else
                    {
                        isRented = true;
                        break;
                    }
                }

            }
            return isRented;
        }

    }
}
