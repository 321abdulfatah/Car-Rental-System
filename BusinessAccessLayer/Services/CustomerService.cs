using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            if (customer != null)
            {
                await _unitOfWork.Customers.CreateAsync(customer);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteCustomerAsync(Guid customerId)
        {
            var customerDetails = await _unitOfWork.Customers.GetAsync(customerId);
            if (customerDetails != null)
            {
                _unitOfWork.Customers.DeleteAsync(customerId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customerDetailsList = await _unitOfWork.Customers.GetAllAsync();
            return customerDetailsList;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            var customerDetails = await _unitOfWork.Customers.GetAsync(customerId);
            if (customerDetails != null)
            {
                return customerDetails;
            }
            return null;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer != null)
            {
                var customerItem = await _unitOfWork.Customers.GetAsync(customer.Id);
                if (customerItem != null)
                {
                    _unitOfWork.Customers.UpdateAsync(customerItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<PaginatedResult<Customer>> GetListCustomersAsync(Expression<Func<Customer, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Customers.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}
