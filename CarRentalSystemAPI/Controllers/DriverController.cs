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
    public class DriverController : ControllerBase
    {
        private readonly IRepository<Driver> _RepositoryDriver;

        public DriverController(IRepository<Driver> RepositoryDriver)
        {
            _RepositoryDriver = RepositoryDriver;
        }

        [HttpGet("getdrivers")]
        public IActionResult GetDrivers(int page, int pageSize)//localhost..../api/cars/getcars
        {
            List<DriverDto> LstDriver = new List<DriverDto>();

            var result = _RepositoryDriver.GetList(page, pageSize);

            foreach (var driver in result)
            {
                DriverDto objdriver = new DriverDto();

                objdriver.Id = driver.Id;
                objdriver.Name = driver.Name;
                objdriver.Gender = driver.Gender;
                objdriver.Age = driver.Age;
                objdriver.Email = driver.Email;
                objdriver.Phone = driver.Phone;
                objdriver.Address = driver.Address;
                objdriver.Salary = driver.Salary;
                objdriver.isAvailable = driver.isAvailable;
                LstDriver.Add(objdriver);
            }
            return Ok(LstDriver);
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {

            var driver = _RepositoryDriver.Get(id);

            DriverDto objdriver = new DriverDto();

            objdriver.Id = driver.Id;
            objdriver.Name = driver.Name;
            objdriver.Gender = driver.Gender;
            objdriver.Age = driver.Age;
            objdriver.Email = driver.Email;
            objdriver.Phone = driver.Phone;
            objdriver.Address = driver.Address;
            objdriver.Salary = driver.Salary;
            objdriver.isAvailable = driver.isAvailable;

            return Ok(objdriver);
        }


        // POST api/<DricerController>
        [HttpPost]
        public IActionResult Create([FromForm] DriverDto DriverDto)
        {

            Driver DriverRequest = new Driver();

            DriverRequest.Id = DriverDto.Id;
            DriverRequest.Name = DriverDto.Name;
            DriverRequest.Gender = DriverDto.Gender;
            DriverRequest.Age = DriverDto.Age;
            DriverRequest.Email = DriverDto.Email;
            DriverRequest.Phone = DriverDto.Phone;
            DriverRequest.Address = DriverDto.Address;
            DriverRequest.Salary = DriverDto.Salary;
            DriverRequest.isAvailable = DriverDto.isAvailable;

            _RepositoryDriver.Create(DriverRequest);

            return Ok();
        }

        // PUT api/<DriverController>/5
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] DriverDto DriverDto)
        {

            try
            {
                if (id != DriverDto.Id)
                    return BadRequest("ID mismatch");

                Driver DriverRequest = new Driver();

                DriverRequest.Id = DriverDto.Id;
                DriverRequest.Name = DriverDto.Name;
                DriverRequest.Gender = DriverDto.Gender;
                DriverRequest.Age = DriverDto.Age;
                DriverRequest.Email = DriverDto.Email;
                DriverRequest.Phone = DriverDto.Phone;
                DriverRequest.Address = DriverDto.Address;
                DriverRequest.Salary = DriverDto.Salary;
                DriverRequest.isAvailable = DriverDto.isAvailable;

                var req = _RepositoryDriver.Update(DriverRequest);

                if (req == null) return NotFound($"Driver with Id = {id} not found");

                return Ok(DriverRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        // DELETE api/<DriverController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _RepositoryDriver.Delete(id);
            return Ok("Record Deleted Successfully");
        }

    }
}
