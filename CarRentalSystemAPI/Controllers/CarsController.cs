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
        public async Task<IActionResult> GetList([FromQuery] CarRequestDto carDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var carDetailsList = await _carService.GetAllCars();
            if (carDetailsList == null)
            {
                return NotFound();
            }

            return Ok(new ApiOkResponse(carDetailsList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var car = await _carService.GetCarById(id);

            if (car != null)
            {
                var carDto = _mapper.Map<CarDto>(car);

                return Ok(new ApiOkResponse(carDto));
            }
            else
            {
                return BadRequest();
            }
            
        }


        // POST api/<CarsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCarDto createCarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            
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
                    return Ok(new ApiOkResponse(carDto));
                }
                else
                {
                    return BadRequest();
                }

            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateCarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var carRequest = _mapper.Map<Car>(carDto);

            var isCarUpdated = await _carService.UpdateCar(carRequest);
            if (isCarUpdated)
            {
                return Ok(new ApiOkResponse(carDto));
            }
            return BadRequest();
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var car = _carService.GetCarById(id);

            var isCarDeleted = await _carService.DeleteCar(id);

            if (isCarDeleted)
            {
                var objCar = _mapper.Map<CarDto>(car);

                return Ok(new ApiOkResponse(objCar));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
