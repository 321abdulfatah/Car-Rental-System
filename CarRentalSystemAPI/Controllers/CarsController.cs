using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using System.Linq.Expressions;

namespace CarRentalSystemAPI.Controllers
{
    //[Authorize]
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

        [HttpGet("getAll")]
        public async Task<List<CarDto>> GetAllCarsAsync()//localhost..../api/cars/getAll
        {
            var carDetailsList = await _carService.GetAllCarsAsync();
           
            var carDtos = _mapper.Map<List<CarDto>>(carDetailsList);

            return carDtos;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<CarDto> GetAsync(Guid id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                var errorMessage = "Car with the specified ID was not found.";
                return new CarDto { ErrorMessage = errorMessage };
            }
            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        [HttpGet]
        public async Task<CarListDto> GetListCarsAsync([FromQuery] CarRequestDto carDto)
        {
            bool foundColumn = true;

            Expression<Func<Car, bool>> filter = car => true; // Initialize the filter to return all records

            if (!string.IsNullOrEmpty(carDto.columnName) && !string.IsNullOrEmpty(carDto.searchTerm))
            {
                /*var propertyInfo = typeof(Car).GetProperty(carDto.columnName);
                if (propertyInfo != null)
                {
                    filter = car => propertyInfo.GetValue(car).ToString().Contains(carDto.searchTerm);
                }*/
                switch (carDto.columnName)
                {
                    case "Type":
                        filter = car => car.Type.Contains(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Color":
                        filter = car => car.Color.Contains(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "DailyFare":
                        filter = car => car.DailyFare.ToString().Equals(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "EngineCapacity":
                        filter = car => car.EngineCapacity.ToString().Equals(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    default:
                        foundColumn = !foundColumn;
                        filter = car => false;
                        break;
                }
            }

            else if (!string.IsNullOrWhiteSpace(carDto.searchTerm))
            {
                filter = car => car.Type.Contains(carDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
            }

            var pagedCars = await _carService.GetListCarsAsync(
                filter,
                carDto.sortBy,
                carDto.isAscending,
                carDto.PageIndex,
                carDto.PageSize
            );
           
            var carDtos = _mapper.Map<List<CarDto>>(pagedCars.Data);
            
            if (!foundColumn)
            {
                var errorMessage = $"Column Name with {carDto.columnName} was not found.";

                return new CarListDto { Data = carDtos, TotalCount = pagedCars.TotalCount, ErrorMessage = errorMessage };
            }
            return new CarListDto { Data = carDtos, TotalCount = pagedCars.TotalCount };
        }

        // POST api/<CarsController>
        [HttpPost]
        public async Task<CarDto> CreateAsync([FromForm] CreateCarDto createCarDto)
        {

            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to create the car due to a validation error." : errorMessage;

                return new CarDto { ErrorMessage = errorMessage };
            }

            var carDto = _mapper.Map<CarDto>(createCarDto);

            var carRequest = _mapper.Map<Car>(carDto);

            var isCarCreated = await _carService.CreateCarAsync(carRequest);

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

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<CarDto> UpdateAsync(Guid id,[FromForm] UpdateCarDto updateCarDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to update the car due to a validation error." : errorMessage;

                return new CarDto { ErrorMessage = errorMessage };
            }

            var existingCar = await _carService.GetCarByIdAsync(id);

            if (existingCar == null)
            {
                var errorMessage = "Car with the specified ID was not found.";

                return new CarDto { ErrorMessage = errorMessage };
            }

            var carDto = _mapper.Map<CarDto>(updateCarDto);

            
            var carRequest = _mapper.Map<Car>(updateCarDto);

            var isCarUpdated = await _carService.UpdateCarAsync(carRequest);
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

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<CarDto> DeleteAsync(Guid id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            var isCarDeleted = await _carService.DeleteCarAsync(id);

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
