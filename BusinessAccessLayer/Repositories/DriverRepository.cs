using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;



namespace BusinessAccessLayer.Repositories
{
    public class DriverRepository : Repository<Driver>, IDriverRepository
    {
        public DriverRepository(CarRentalDBContext dbContext) : base(dbContext)
        {

        }
    }
   
}
