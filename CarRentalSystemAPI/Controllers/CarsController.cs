using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("getcars")]
        public CarListDto GetList([FromQuery] CarRequestDto CarDto)//localhost..../api/cars/getcars
        {

            List<CarDto> results = new List<CarDto>();

            var result = _RepositoryCar.GetList(CarDto.PageNumber, CarDto.PageSize);

            foreach (var car in result)
            {
                CarDto objcar = new CarDto();

                objcar.Id = car.Id;
                objcar.DriverId = car.DriverId;
                objcar.DailyFare = car.DailyFare;
                objcar.Color = car.Color;
                objcar.EngineCapacity = car.EngineCapacity;
                objcar.Type = car.Type;

                results.Add(objcar);
            }
            CarListDto LstCar = new CarListDto();
            LstCar.pageNumber = CarDto.PageNumber;
            LstCar.pageSize = CarDto.PageSize;
            LstCar.results = results;

            return LstCar;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public CarDto Get(Guid id)
        {

            var car = _RepositoryCar.Get(id);

            CarDto objcar = new CarDto();

            objcar.Id = car.Id;
            objcar.DriverId = car.DriverId;
            objcar.DailyFare = car.DailyFare;
            objcar.Color = car.Color;
            objcar.EngineCapacity = car.EngineCapacity;
            objcar.Type = car.Type;

            return objcar;
        }


        // POST api/<CarsController>
        [HttpPost]
        public CreateCarDto Create([FromForm] CreateCarDto CarDto)
        {
            Car CarRequest = new Car();

            CarRequest.Id = Guid.NewGuid();
            CarRequest.DriverId = CarDto.DriverId;
            CarRequest.EngineCapacity = CarDto.EngineCapacity;
            CarRequest.Color = CarDto.Color;
            CarRequest.Type = CarDto.Type;
            CarRequest.DailyFare = CarDto.DailyFare;

            _RepositoryCar.Create(CarRequest);

            return CarDto;
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public UpdateCarDto Update(Guid id, [FromForm] UpdateCarDto CarDto)
        {

            try
            {
                Car CarRequest = new Car();

                CarRequest.Id = CarDto.Id;
                CarRequest.DriverId = CarDto.DriverId;
                CarRequest.EngineCapacity = CarDto.EngineCapacity;
                CarRequest.Color = CarDto.Color;
                CarRequest.Type = CarDto.Type;
                CarRequest.DailyFare = CarDto.DailyFare;

                var req = _RepositoryCar.Update(CarRequest);


                return CarDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public CarDto Delete(Guid id)
        {
            var car = _RepositoryCar.Get(id);

            CarDto objcar = new CarDto();

            objcar.Id = car.Id;
            objcar.DriverId = car.DriverId;
            objcar.DailyFare = car.DailyFare;
            objcar.Color = car.Color;
            objcar.EngineCapacity = car.EngineCapacity;
            objcar.Type = car.Type;

            _RepositoryCar.Delete(id);

            return objcar;
        }
    }
}
