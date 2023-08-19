using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateCustomer(Customer customer)
        {
            if (customer != null)
            {
                await _unitOfWork.Customers.Create(customer);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteCustomer(Guid customerId)
        {
            var customerDetails = await _unitOfWork.Customers.Get(customerId);
            if (customerDetails != null)
            {
                _unitOfWork.Customers.Delete(customerId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customerDetailsList = await _unitOfWork.Customers.GetList();
            return customerDetailsList;
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            var customerDetails = await _unitOfWork.Customers.Get(customerId);
            if (customerDetails != null)
            {
                return customerDetails;
            }
            return null;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (customer != null)
            {
                var customerItem = await _unitOfWork.Customers.Get(customer.Id);
                if (customerItem != null)
                {
                    _unitOfWork.Customers.Update(customerItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
