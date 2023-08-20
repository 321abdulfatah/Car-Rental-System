using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<PaginatedResult<Customer>> GetListAsync(Expression<Func<Customer, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
