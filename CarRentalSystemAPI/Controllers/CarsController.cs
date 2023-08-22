using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using System.Linq.Expressions;
using Abp.Domain.Entities;

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

            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        [HttpGet]
        public async Task<CarListDto> GetListCarsAsync([FromQuery] CarRequestDto carDto)
        {
            var pagedCars = await _carService.GetListCarsAsync(
                carDto.SearchTerm,
                carDto.SortBy,
                carDto.PageIndex,
                carDto.PageSize
            );
           
            var carDtos = _mapper.Map<List<CarDto>>(pagedCars.Data);
            
            return new CarListDto { Data = carDtos, TotalCount = pagedCars.TotalCount };
        }

        // POST api/<CarsController>
        [HttpPost]
        public async Task<CreateCarDto> CreateAsync([FromForm] CreateCarDto createCarDto)
        {
            var carRequest = _mapper.Map<Car>(createCarDto);

            var isCarCreated = await _carService.CreateCarAsync(carRequest);

            if (isCarCreated)
            {
                return createCarDto;
            }
            else
            {
                var errorMessage = "Failed to create the car due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<UpdateCarDto> UpdateAsync(Guid id,[FromForm] UpdateCarDto updateCarDto)
        {
            var carRequest = _mapper.Map<Car>(updateCarDto);

            var isCarUpdated = await _carService.UpdateCarAsync(carRequest);

            if (isCarUpdated)
            {
                return updateCarDto;
            }
            else
            {
                var errorMessage = "Failed to update the car due to a validation error.";
                throw new InvalidOperationException(errorMessage);
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
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
