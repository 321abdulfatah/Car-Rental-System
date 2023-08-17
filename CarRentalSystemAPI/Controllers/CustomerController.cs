using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BusinessAccessLayer.Data.Validate;
using Microsoft.AspNetCore.Authorization;
using BusinessAccessLayer.Services.Interfaces;


namespace CarRentalSystemAPI.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CustomerRequestDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customerDetailsList = await _customerService.GetAllCustomers();
            if (customerDetailsList == null)
            {
                return NotFound();
            }

            return Ok(new ApiOkResponse(customerDetailsList));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customer = await _customerService.GetCustomerById(id);

            if (customer != null)
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);

                return Ok(new ApiOkResponse(customerDto));
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customerDto = _mapper.Map<CustomerDto>(createCustomerDto);

            var customerValidator = new CustomerValidator();

            var result = customerValidator.Validate(customerDto);

            if (result.IsValid)
            {

                var customerRequest = _mapper.Map<Customer>(customerDto);

                var isCustomerCreated = await _customerService.CreateCustomer(customerRequest);

                if (isCustomerCreated)
                {
                    return Ok(new ApiOkResponse(customerDto));
                }
                else
                {
                    return BadRequest();
                }

            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customerRequest = _mapper.Map<Customer>(customerDto);

            var isCustomerUpdated = await _customerService.UpdateCustomer(customerRequest);
            if (isCustomerUpdated)
            {
                return Ok(new ApiOkResponse(customerDto));
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customer = _customerService.GetCustomerById(id);

            var isCustomerDeleted = await _customerService.DeleteCustomer(id);

            if (isCustomerDeleted)
            {
                var objCustomer = _mapper.Map<CustomerDto>(customer);

                return Ok(new ApiOkResponse(objCustomer));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
