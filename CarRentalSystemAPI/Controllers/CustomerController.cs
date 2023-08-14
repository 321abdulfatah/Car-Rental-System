using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getcustomers")]
        public ActionResult<CustomerListDto> GetList([FromQuery] CustomerRequestDto CustomerDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customerDetailsList = _unitOfWork.Customers.GetList(CustomerDto.Search, CustomerDto.Column, CustomerDto.SortOrder, CustomerDto.OrderBy, CustomerDto.PageIndex, CustomerDto.PageSize);
            if (customerDetailsList == null)
            {
                return NotFound();
            }

            var Lstcustomer = _mapper.Map<List<CustomerDto>>(customerDetailsList.Items);

            CustomerListDto customerList = new CustomerListDto();

            customerList.Items = Lstcustomer;
            customerList.TotalRows = customerDetailsList.TotalRows;
            customerList.TotalPages = customerDetailsList.TotalPages;


            return Ok(new ApiOkResponse(customerList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<CustomerDto> Get(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customer = _unitOfWork.Customers.Get(Id);

            if (customer == null)
            {
                return NotFound(new ApiResponse(404, $"Customer not found with id {Id}"));
            }
            var objcustomer = _mapper.Map<CustomerDto>(customer);

            return Ok(new ApiOkResponse(objcustomer));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateCustomerDto> Create([FromForm] CreateCustomerDto CustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var CustomerRequest = _mapper.Map<Customer>(CustomerDto);
            
            _unitOfWork.Customers.Create(CustomerRequest);

            return Ok(new ApiOkResponse(CustomerDto));
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateCustomerDto> Update([FromForm] UpdateCustomerDto CustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var CustomerRequest = _mapper.Map<Customer>(CustomerDto);

            _unitOfWork.Customers.Update(CustomerRequest);

            return Ok(new ApiOkResponse(CustomerDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<CustomerDto> Delete(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customer = _unitOfWork.Customers.Get(Id);

            if (customer == null)
            {
                return NotFound(new ApiResponse(404, $"Customer not found with id {Id}"));
            }
            var objcustomer = _mapper.Map<CarDto>(customer);

            _unitOfWork.Customers.Delete(Id);

            return Ok(new ApiOkResponse(objcustomer));
        }
    }
}
