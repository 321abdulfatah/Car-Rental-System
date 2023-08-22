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
        public CarRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
        }
    }
}
