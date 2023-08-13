using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Repositories
{
    public class CarRepository : Repository < Car > , ICarRepository
    {
        public CarRepository(CarRentalDBContext dbContext) : base(dbContext)
        {

        }

    }
}
