using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalSystemAPI.FileUploadExtension;
using Microsoft.AspNetCore.Http.Extensions;
using CarRentalSystemAPI.Dtos;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IRepository<Car> _RepositoryCar;
        public CarsController(IRepository<Car> RepositryCar)
        {
            _RepositoryCar = RepositryCar;
        }
        
        [HttpGet("getall")]
        public IActionResult GetAll()//localhost..../api/cars/getall
        {
            // sorting on  Engine Capacity
            List<CarDto> LstCar = new List<CarDto>();

            var result = _RepositoryCar.GetAll().OrderBy(c=>c.EngineCapacity).ToList();

            foreach(var car in result){
                CarDto objcar = new CarDto();

                objcar.Id = car.Id;
                objcar.DriverId = car.DriverId;
                objcar.Driver = car.Driver;
                objcar.DailyFare = car.DailyFare;
                objcar.Color = car.Color;
                objcar.EngineCapacity = car.EngineCapacity;
                objcar.Type = car.Type;
                LstCar.Add(objcar);
            }
            return Ok(LstCar);
        }

        [HttpGet("getcars")]
        public IActionResult GetCars(int page,int pageSize)//localhost..../api/cars/getcars
        {
            List<CarDto> LstCar = new List<CarDto>();

            var result = _RepositoryCar.GetAll(page, pageSize);

            foreach (var car in result)
            {
                CarDto objcar = new CarDto();

                objcar.Id = car.Id;
                objcar.DriverId = car.DriverId;
                objcar.Driver = car.Driver;
                objcar.DailyFare = car.DailyFare;
                objcar.Color = car.Color;
                objcar.EngineCapacity = car.EngineCapacity;
                objcar.Type = car.Type;
                LstCar.Add(objcar);
            }
            return Ok(LstCar);
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {

            var car = _RepositoryCar.GetById(id);
            
            CarDto objcar = new CarDto();

            objcar.Id = car.Id;
            objcar.DriverId = car.DriverId;
            objcar.Driver = car.Driver;
            objcar.DailyFare = car.DailyFare;
            objcar.Color = car.Color;
            objcar.EngineCapacity = car.EngineCapacity;
            objcar.Type = car.Type;

            return Ok(objcar);
        }


        // POST api/<CarsController>
        [HttpPost]
        public IActionResult Post([FromForm] CarDto CarDto)
        {
            
            if (CarDto == null)
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S01", Error = "Invalid post request" });
            }

            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
            {
                return BadRequest(new PostResponse { Success = false, ErrorCode = "S02", Error = "Invalid post header" });
            }

            Car CarRequest = new Car();

            CarRequest.Id = CarDto.Id;
            CarRequest.DriverId = CarDto.DriverId;
            CarRequest.Driver = CarDto.Driver;
            CarRequest.EngineCapacity = CarDto.EngineCapacity;
            CarRequest.Color = CarDto.Color;
            CarRequest.Type = CarDto.Type;
            CarRequest.DailyFare = CarDto.DailyFare;

            _RepositoryCar.Create(CarRequest);
            return Ok();
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] CarDto CarDto)
        {

            try
            {
                if (id != CarDto.Id)
                    return BadRequest("ID mismatch");

                Car CarRequest = new Car();

                CarRequest.Id = CarDto.Id;
                CarRequest.DriverId = CarDto.DriverId;
                CarRequest.Driver = CarDto.Driver;
                CarRequest.EngineCapacity = CarDto.EngineCapacity;
                CarRequest.Color = CarDto.Color;
                CarRequest.Type = CarDto.Type;
                CarRequest.DailyFare = CarDto.DailyFare;

                var req = _RepositoryCar.Update(CarRequest);

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
            _RepositoryCar.Delete(id);
            return Ok("Record Deleted Successfully");
        }
    }
}
