using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using System.Linq.Expressions;

namespace CarRentalSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet("getAll")]
        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customerDetailsList = await _customerService.GetAllCustomersAsync();
            
            var customerDtos = _mapper.Map<List<CustomerDto>>(customerDetailsList);

            return customerDtos;
        }

        [HttpGet("{id}")]
        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                var errorMessage = "Customer with the specified ID was not found.";
                return new CustomerDto { ErrorMessage = errorMessage };
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);

            return customerDto;

        }
        [HttpGet]
        public async Task<PaginatedResult<CustomerDto>> GetListCustomersAsync([FromQuery] CustomerRequestDto customerDto)
        {
            Expression<Func<Customer, bool>> filter = customer => true; // Initialize the filter to return all records

            if (!string.IsNullOrEmpty(customerDto.columnName) && !string.IsNullOrEmpty(customerDto.searchTerm))
            {
                var propertyInfo = typeof(Customer).GetProperty(customerDto.columnName);
                if (propertyInfo != null)
                {
                    filter = customer => propertyInfo.GetValue(customer).ToString().Contains(customerDto.searchTerm);
                }
            }

            else if (!string.IsNullOrWhiteSpace(customerDto.searchTerm))
            {
                filter = customer => customer.Name.Contains(customerDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
            }

            var pagedCustomers = await _customerService.GetListCustomersAsync(
                filter,
                customerDto.sortBy,
                customerDto.isAscending,
                customerDto.PageIndex,
                customerDto.PageSize
            );

            var customerDtos = _mapper.Map<List<CustomerDto>>(pagedCustomers.Data);

            var result = new PaginatedResult<CustomerDto>
            {
                Data = customerDtos,
                TotalCount = pagedCustomers.TotalCount
            };
            return result;
        }
        [HttpPost]
        public async Task<CustomerDto> CreateAsync([FromForm] CreateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to create the customer due to a validation error." : errorMessage;

                return new CustomerDto { ErrorMessage = errorMessage };
            }

            var customerDto = _mapper.Map<CustomerDto>(createCustomerDto);

            var customerRequest = _mapper.Map<Customer>(customerDto);

            var isCustomerCreated = await _customerService.CreateCustomerAsync(customerRequest);

            if (isCustomerCreated)
            {
                return customerDto;
            }
            else
            {
                var errorMessage = "Failed to create the customer due to a validation error.";

                return new CustomerDto { ErrorMessage = errorMessage };
            }
        }

        [HttpPut("{id}")]
        public async Task<CustomerDto> UpdateAsync(Guid id, [FromForm] UpdateCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to update the customer due to a validation error." : errorMessage;

                return new CustomerDto { ErrorMessage = errorMessage };
            }
            
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);

            if (existingCustomer == null)
            {
                var errorMessage = "Customer with the specified ID was not found.";

                return new CustomerDto { ErrorMessage = errorMessage };
            }

            var customerDto = _mapper.Map<CustomerDto>(updateCustomerDto);

            var customerRequest = _mapper.Map<Customer>(updateCustomerDto);

            var isCustomerUpdated = await _customerService.UpdateCustomerAsync(customerRequest);
            if (isCustomerUpdated)
            {
                return customerDto;
            }
            else
            {
                var errorMessage = "Failed to update the customer due to a validation error.";

                return new CustomerDto { ErrorMessage = errorMessage };
            }
        }

        [HttpDelete("{id}")]
        public async Task<CustomerDto> DeleteAsync(Guid id)
        {
            var customer = _customerService.GetCustomerByIdAsync(id);

            var isCustomerDeleted = await _customerService.DeleteCustomerAsync(id);

            if (isCustomerDeleted)
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);

                return customerDto;
            }
            else
            {
                var errorMessage = "Customer with the specified ID can not delete.";
                return new CustomerDto { ErrorMessage = errorMessage };
            }
        }
    }
}
