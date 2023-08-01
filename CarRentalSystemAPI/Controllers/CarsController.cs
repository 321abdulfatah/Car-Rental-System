﻿using BusinessAccessLayer.Services;
using DataAccessLayer.Models;
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
        private readonly IServiceCar<Car> _ServiceCar;
        public CarsController(IServiceCar<Car> ServiceCar)
        {
            _ServiceCar = ServiceCar;
        }
        
        [HttpGet("getall")]
        public IActionResult GetAll()//localhost..../api/cars/getall
        {
            // sorting on  Engine Capacity
            List<CarDto> LstCar = new List<CarDto>();

            var result = _ServiceCar.GetAll().OrderBy(c=>c.EngineCapacity).ToList();

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

        [HttpGet("getall/{Type}")]
        public IActionResult GetAll(string Type)//localhost..../api/cars/getall/{Type}
        {
            List<CarDto> LstCar = new List<CarDto>();

            var result = _ServiceCar.GetAll(Type);

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

        [HttpGet("getcars")]
        public IActionResult GetCars(int page,int pageSize)//localhost..../api/cars/getcars
        {
            List<CarDto> LstCar = new List<CarDto>();

            var result = _ServiceCar.GetAll(page, pageSize);

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

            var car =  _ServiceCar.GetById(id);
            
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

            _ServiceCar.Create(CarRequest);
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