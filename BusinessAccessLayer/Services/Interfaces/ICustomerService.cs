using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CreateCustomerAsync(Customer customer);

        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer> GetCustomerByIdAsync(Guid customerId);

        Task<bool> UpdateCustomerAsync(Customer customer);

        Task<bool> DeleteCustomerAsync(Guid customerId);
        Task<PaginatedResult<Customer>> GetListCustomersAsync(Expression<Func<Customer, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize);

    }
}
