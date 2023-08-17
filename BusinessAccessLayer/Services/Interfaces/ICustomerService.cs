using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CreateCustomer(Customer customer);

        Task<IEnumerable<Customer>> GetAllCustomers();

        Task<Customer> GetCustomerById(Guid customerId);

        Task<bool> UpdateCustomer(Customer customer);

        Task<bool> DeleteCustomer(Guid customerId);
    }
}
