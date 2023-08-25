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
    //[Authorize]
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

        [HttpGet("{id}")]
        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return customerDto;

        }
        [HttpGet]
        public async Task<CustomerListDto> GetListCustomersAsync([FromQuery] CustomerRequestDto customerDto)
        {
            var pagedCustomeras = await _customerService.GetListCustomersAsync(
                customerDto.SearchTerm,
                customerDto.SortBy,
                customerDto.PageIndex,
                customerDto.PageSize
            );

            var customerDtos = _mapper.Map<List<CustomerDto>>(pagedCustomeras.Data);

            return new CustomerListDto { Data = customerDtos, TotalCount = pagedCustomeras.TotalCount };
        }
        [HttpPost]
        public async Task<CreateCustomerDto> CreateAsync([FromForm] CreateCustomerDto createCustomerDto)
        {
            var customerRequest = _mapper.Map<Customer>(createCustomerDto);

            var isCustomerCreated = await _customerService.CreateCustomerAsync(customerRequest);

            if (isCustomerCreated)
            {
                return createCustomerDto;
            }
            else
            {
                var errorMessage = "Failed to create the cutomer due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        [HttpPut("{id}")]
        public async Task<UpdateCustomerDto> UpdateAsync(Guid id, [FromForm] UpdateCustomerDto updateCustomerDto)
        {
            var customerRequest = _mapper.Map<Customer>(updateCustomerDto);

            var isCustomerUpdated = await _customerService.UpdateCustomerAsync(customerRequest);
            if (isCustomerUpdated)
            {
                return updateCustomerDto;
            }
            else
            {
                var errorMessage = "Failed to update the customer due to a validation error.";
                throw new InvalidOperationException(errorMessage);
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
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
