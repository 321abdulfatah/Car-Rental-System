using BusinessAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;


namespace BusinessAccessLayer.Repositories
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        public UsersRepository(CarRentalDBContext dbContext) : base(dbContext)
        {

        }
    }
}
