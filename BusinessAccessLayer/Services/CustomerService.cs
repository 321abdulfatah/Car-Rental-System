using Abp.Domain.Entities;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
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
                await _unitOfWork.Customers.DeleteAsync(customerId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            var customerDetails = await _unitOfWork.Customers.GetAsync(customerId);
            if (customerDetails == null)
            {
                throw new EntityNotFoundException($"Customer with ID {customerId} not found.");
            }
            return customerDetails;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer != null)
            {
                await _unitOfWork.Customers.UpdateAsync(customer);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
                
            }
            return false;
        }

        public async Task<PaginatedResult<Customer>> GetListCustomersAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {
            Expression<Func<Customer, bool>> filter = customer => true;

            if (!string.IsNullOrEmpty(searchTerm))
                filter = customer => customer.Address.Contains(searchTerm)          || 
                                     customer.Name.Contains(searchTerm)             ||
                                     customer.Phone.ToString().Equals(searchTerm)   ||
                                     customer.Age.ToString().Equals(searchTerm)     ||
                                     customer.Email.Equals(searchTerm)              ||
                                     customer.Gender.Equals(searchTerm) ;

            var query = _unitOfWork.Customers.GetAll();

            // Apply filter
            query = query.Where(filter);

            // Calculate total count
            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                // Split the sortBy string into column name and sorting direction
                var sortByParts = sortBy.Split(' ');
                string columnName = sortByParts[0];
                string sortingDirection = sortByParts.Length > 1 ? sortByParts[1] : "asc"; // Default to ascending

                // Determine the sorting order
                bool isDescending = sortingDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

                switch (columnName.ToLower())
                {
                    case "address":
                        query = isDescending ? query.OrderByDescending(c => c.Address) : query.OrderBy(c => c.Address);
                        break;
                    case "name":
                        query = isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                        break;
                    case "age":
                        query = isDescending ? query.OrderByDescending(c => c.Age) : query.OrderBy(c => c.Age);
                        break;
                    case "phone":
                        query = isDescending ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }

            var pagedCustomers = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Customer>
            {
                Data = pagedCustomers,
                TotalCount = totalCount
            };
        }
    }
}
