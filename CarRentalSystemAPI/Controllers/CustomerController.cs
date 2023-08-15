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

        [HttpGet]
        public ActionResult<CustomerListDto> GetList([FromQuery] CustomerRequestDto customerDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customerDetailsList = _unitOfWork.Customers.GetList(customerDto.Search, customerDto.Column, customerDto.SortOrder, customerDto.OrderBy, customerDto.PageIndex, customerDto.PageSize);
            if (customerDetailsList == null)
            {
                return NotFound();
            }

            var lstCustomer = _mapper.Map<List<CustomerDto>>(customerDetailsList.Items);

            CustomerListDto customerList = new CustomerListDto();

            customerList.Items = lstCustomer;
            customerList.TotalRows = customerDetailsList.TotalRows;
            customerList.TotalPages = customerDetailsList.TotalPages;


            return Ok(new ApiOkResponse(customerList));
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerDto> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var customer = _unitOfWork.Customers.Get(id);

            var objCustomer = _mapper.Map<CustomerDto>(customer);

            return Ok(new ApiOkResponse(objCustomer));
        }


        [HttpPost]
        public ActionResult<CreateCustomerDto> Create([FromForm] CreateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customerRequest = _mapper.Map<Customer>(customerDto);
            
            _unitOfWork.Customers.Create(customerRequest);

            return Ok(new ApiOkResponse(customerDto));
        }

        [HttpPut("{id}")]
        public ActionResult<UpdateCustomerDto> Update([FromForm] UpdateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customerRequest = _mapper.Map<Customer>(customerDto);

            _unitOfWork.Customers.Update(customerRequest);

            return Ok(new ApiOkResponse(customerDto));

        }

        [HttpDelete("{id}")]
        public ActionResult<CustomerDto> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var customer = _unitOfWork.Customers.Get(id);

            var objCustomer = _mapper.Map<CarDto>(customer);

            _unitOfWork.Customers.Delete(id);

            return Ok(new ApiOkResponse(objCustomer));
        }
    }
}
