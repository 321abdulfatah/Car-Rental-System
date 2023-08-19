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
using DataAccessLayer.Common.Models;
using System.Linq.Expressions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRentalSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly ICarService _carService;

        public CarsController(ICarService carService, IMapper mapper)
        {
            _mapper = mapper;
            _carService = carService;
        }

        [HttpGet]
        public async Task<List<CarDto>> GetList()//localhost..../api/cars/getcars
        {

            var carDetailsList = await _carService.GetAllCars();
           
            var carDtos = _mapper.Map<List<CarDto>>(carDetailsList);

            return carDtos;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<CarDto> Get(Guid id)
        {
            var car = await _carService.GetCarById(id);

            if (car == null)
            {
                var errorMessage = "Car with the specified ID was not found.";
                return new CarDto { ErrorMessage = errorMessage };
            }
            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        [HttpGet("filtered-sorted")]
        public async Task<PaginatedResult<CarDto>> GetFilteredAndSortedCars([FromQuery] CarRequestDto carDto)
        {
            Expression<Func<Car, bool>> filter = car => true; // Initialize the filter to return all records

            if (!string.IsNullOrWhiteSpace(carDto.searchTerm))
            {
                filter = car => car.Type.Contains(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
            }

            var pagedCars = await _carService.GetFilteredAndSortedCars(
                filter,
                carDto.sortBy,
                carDto.isAscending,
                carDto.PageIndex,
                carDto.PageSize
            );
           
            var carDtos = _mapper.Map<List<CarDto>>(pagedCars.Data);

            var result = new PaginatedResult<CarDto>
            {
                Data = carDtos,
                TotalCount = pagedCars.TotalCount
            };
            return result;
        }
        // POST api/<CarsController>
        [HttpPost]
        public async Task<CarDto> Create([FromForm] CreateCarDto createCarDto)
        {
            
            var carDto = _mapper.Map<CarDto>(createCarDto);

            var carValidator = new CarValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated

            var result = carValidator.Validate(carDto);

            if (result.IsValid)
            {

                var carRequest = _mapper.Map<Car>(carDto);

                var isCarCreated = await _carService.CreateCar(carRequest);

                if (isCarCreated)
                {
                    return carDto;
                }
                else
                {
                    var errorMessage = "Failed to create the car due to a validation error.";

                    return new CarDto { ErrorMessage = errorMessage };
                }

            }
            else
            {
                var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToString();

                return new CarDto { ErrorMessage = errorMessage };
            }
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<CarDto> Update(Guid id,[FromForm] UpdateCarDto updateCarDto)
        {
            var existingCar = await _carService.GetCarById(id);

            if (existingCar == null)
            {
                var errorMessage = "Car with the specified ID was not found.";

                return new CarDto { ErrorMessage = errorMessage };
            }

            var carDto = _mapper.Map<CarDto>(updateCarDto);

            var carValidator = new CarValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated

            var result = carValidator.Validate(carDto);

            if (result.IsValid)
            {

                var carRequest = _mapper.Map<Car>(updateCarDto);

                var isCarUpdated = await _carService.UpdateCar(carRequest);
                if (isCarUpdated)
                {
                    return carDto;
                }
                else
                {
                    var errorMessage = "Failed to update the car due to a validation error.";

                    return new CarDto { ErrorMessage = errorMessage };
                }

            }
            else
            {
                var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToString();

                return new CarDto { ErrorMessage = errorMessage };
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<CarDto> Delete(Guid id)
        {
            var car = await _carService.GetCarById(id);

            var isCarDeleted = await _carService.DeleteCar(id);

            if (isCarDeleted)
            {
                var carDto = _mapper.Map<CarDto>(car);

                return carDto;
            }
            else
            {
                var errorMessage = "Car with the specified ID can not delete.";
                return new CarDto { ErrorMessage = errorMessage };
            }
        }
    }
}
