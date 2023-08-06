using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalSystemAPI.Dtos;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _RepositoryCustomer;

        public CustomerController(IRepository<Customer> RepositoryCustomer)
        {
            _RepositoryCustomer = RepositoryCustomer;
        }

        [HttpGet("getcustomers")]
        public IActionResult GetCustomers(int page, int pageSize)//localhost..../api/cars/getcars
        {
            List<CustomerDto> LstCustomer = new List<CustomerDto>();

            var result = _RepositoryCustomer.GetList(page, pageSize);

            foreach (var customer in result)
            {
                CustomerDto objcustomer = new CustomerDto();

                objcustomer.Id = customer.Id;
                objcustomer.Name = customer.Name;
                objcustomer.Gender = customer.Gender;
                objcustomer.Age = customer.Age;
                objcustomer.Email = customer.Email;
                objcustomer.Phone = customer.Phone;
                objcustomer.Address = customer.Address;
                
                LstCustomer.Add(objcustomer);
            }
            return Ok(LstCustomer);
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {

            var customer = _RepositoryCustomer.Get(id);

            CustomerDto objcustomer = new CustomerDto();

            objcustomer.Id = customer.Id;
            objcustomer.Name = customer.Name;
            objcustomer.Gender = customer.Gender;
            objcustomer.Age = customer.Age;
            objcustomer.Email = customer.Email;
            objcustomer.Phone = customer.Phone;
            objcustomer.Address = customer.Address;
            

            return Ok(objcustomer);
        }


        // POST api/<CarsController>
        [HttpPost]
        public IActionResult Create([FromForm] CreateCustomerDto CustomerDto)
        {
            Customer CustomerRequest = new Customer();

            CustomerRequest.Id = Guid.NewGuid();
            CustomerRequest.Name = CustomerDto.Name;
            CustomerRequest.Gender = CustomerDto.Gender;
            CustomerRequest.Age = CustomerDto.Age;
            CustomerRequest.Email = CustomerDto.Email;
            CustomerRequest.Phone = CustomerDto.Phone;
            CustomerRequest.Address = CustomerDto.Address;
            

            _RepositoryCustomer.Create(CustomerRequest);

            return Ok();
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] CustomerDto CustomerDto)
        {

            try
            {
                if (id != CustomerDto.Id)
                    return BadRequest("ID mismatch");

                Customer CustomerRequest = new Customer();

                CustomerRequest.Id = CustomerDto.Id;
                CustomerRequest.Name = CustomerDto.Name;
                CustomerRequest.Gender = CustomerDto.Gender;
                CustomerRequest.Age = CustomerDto.Age;
                CustomerRequest.Email = CustomerDto.Email;
                CustomerRequest.Phone = CustomerDto.Phone;
                CustomerRequest.Address = CustomerDto.Address;
             

                var req = _RepositoryCustomer.Update(CustomerRequest);

                if (req == null) return NotFound($"Customer with Id = {id} not found");

                return Ok(CustomerRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _RepositoryCustomer.Delete(id);
            return Ok("Record Deleted Successfully");
        }
    }
}
