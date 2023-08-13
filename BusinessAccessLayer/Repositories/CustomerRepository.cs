using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;


namespace BusinessAccessLayer.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CarRentalDBContext dbContext) : base(dbContext)
        {

        }

    }
}
