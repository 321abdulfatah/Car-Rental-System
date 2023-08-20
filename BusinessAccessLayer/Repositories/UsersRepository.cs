using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Repositories
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        private readonly CarRentalDBContext _dbContext;

        public UsersRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PaginatedResult<Users>> GetListAsync(Expression<Func<Users, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Users.Where(filter);

            switch (sortBy)
            {
                case "name":
                    query = isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var pagedUsers= await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Users>
            {
                Data = pagedUsers,
                TotalCount = await query.CountAsync()
            };
        }
    }
}
