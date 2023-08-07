using AutoMapper;
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
        private readonly IMapper _mapper;

        private readonly IRepository<Car> _RepositoryCar;
        public CarsController(IRepository<Car> RepositryCar, IMapper mapper)
        {
            _RepositoryCar = RepositryCar;
            _mapper = mapper;

        }

        [HttpGet("getcars")]
        public CarListDto GetList([FromQuery] CarRequestDto CarDto)//localhost..../api/cars/getcars
        {

            var results = _RepositoryCar.GetList(CarDto.PageNumber, CarDto.PageSize);
            
            CarListDto LstCar = new CarListDto();
            LstCar.pageNumber = CarDto.PageNumber;
            LstCar.pageSize = CarDto.PageSize;
            LstCar.results = (List<CarDto>)results;

            return LstCar;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public CarDto Get(Guid id)
        {

            var car = _RepositoryCar.Get(id);

            var objcar = _mapper.Map<CarDto>(car);

            return objcar;
        }


        // POST api/<CarsController>
        [HttpPost]
        public CreateCarDto Create([FromForm] CreateCarDto CarDto)
        {
            var CarRequest = _mapper.Map<Car>(CarDto);

            _RepositoryCar.Create(CarRequest);

            return CarDto;
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public UpdateCarDto Update(Guid id, [FromForm] UpdateCarDto CarDto)
        {

            try
            {
                var CarRequest = _mapper.Map<Car>(CarDto);

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

            var objcar = _mapper.Map<CarDto>(car);

            _RepositoryCar.Delete(id);

            return objcar;
        }
    }
}
