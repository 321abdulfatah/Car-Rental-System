using BusinessAccessLayer.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalSystemAPI.FileUploadExtension;
using Microsoft.AspNetCore.Http.Extensions;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IServiceCar<Car> _ServiceCar;
        public CarsController(IServiceCar<Car> ServiceCar)
        {
            _ServiceCar = ServiceCar;
        }
        
        [HttpGet("getall")]
        public IActionResult GetAll()//localhost..../api/cars/getall
        {
            // sorting on  Engine Capacity
            var result = _ServiceCar.GetAll().OrderBy(c=>c.EngineCapacity).ToList();
            return Ok(result);
            
        }

        [HttpGet("getall/{Type}")]
        public IActionResult GetAll(string Type)//localhost..../api/cars/getall/{Type}
        {
            // sorting on  Engine Capacity
            var result = _ServiceCar.GetAll(Type);
            return Ok(result);

        }
        [HttpGet("getcars")]
        public IActionResult GetCars(int page,int pageSize)//localhost..../api/cars/getcars
        {
            // sorting on  Engine Capacity
            var result = _ServiceCar.GetAll(page, pageSize);
            return Ok(result);

        }
        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var result =  _ServiceCar.GetById(id);
            return Ok(result);
        }


        // POST api/<CarsController>
        [HttpPost]
        public IActionResult Post([FromForm] Car CarRequest)
        {
            
            if (CarRequest == null)
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S01", Error = "Invalid post request" });
            }

            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S02", Error = "Invalid post header" });
            }

            _ServiceCar.Create(CarRequest);
            return Ok();
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] Car CarRequest)
        {

            try
            {
                if (id != CarRequest.Id)
                    return BadRequest("ID mismatch");

                var req = _ServiceCar.Update(CarRequest);
                if (req == null) return NotFound($"Car with Id = {id} not found");
                return Ok(CarRequest);
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
            _ServiceCar.Delete(id);
            return Ok("Record Deleted Successfully");
        }
    }
}
