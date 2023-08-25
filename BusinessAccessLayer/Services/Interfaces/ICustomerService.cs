using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CreateCustomerAsync(Customer customer);

        Task<Customer> GetCustomerByIdAsync(Guid customerId);
        
        Task<bool> UpdateCustomerAsync(Customer customer);
        
        Task<bool> DeleteCustomerAsync(Guid customerId);

        Task<PaginatedResult<Customer>> GetListCustomersAsync(string searchTerm, string sortBy, int pageIndex, int pageSize);

    }
}
