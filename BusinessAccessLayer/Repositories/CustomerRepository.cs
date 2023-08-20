using BusinessAccessLayer.Data;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly CarRentalDBContext _dbContext;

        public CustomerRepository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PaginatedResult<Customer>> GetListAsync(Expression<Func<Customer, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Customers.Where(filter);

            switch (sortBy)
            {
                case "name":
                    query = isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "age":
                    query = isAscending ? query.OrderBy(c => c.Age) : query.OrderByDescending(c => c.Age);
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var pagedCustomers= await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Customer>
            {
                Data = pagedCustomers,
                TotalCount = await query.CountAsync()
            };
        }

    }
}
