using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;


namespace BusinessAccessLayer.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(CarRentalDBContext dbContext) : base(dbContext)
        {

        }
    }
}
