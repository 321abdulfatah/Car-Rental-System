using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;
using BusinessAccessLayer.Data.Validate;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRentalSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;


        public CarsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getcars")]
        public ActionResult<CarListDto> GetList([FromQuery] CarRequestDto CarDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var carDetailsList = _unitOfWork.Cars.GetList(CarDto.Search, CarDto.Column, CarDto.SortOrder, CarDto.OrderBy, CarDto.PageIndex, CarDto.PageSize);
            if (carDetailsList == null)
            {
                return NotFound();
            }

            var Lstcar = _mapper.Map<List<CarDto>>(carDetailsList.Items);

            CarListDto carList =  new CarListDto();

            carList.Items = Lstcar;
            carList.TotalRows = carDetailsList.TotalRows;
            carList.TotalPages = carDetailsList.TotalPages;


            return Ok(new ApiOkResponse(carList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<CarDto> Get(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var car = _unitOfWork.Cars.Get(Id);

            if (car == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objcar = _mapper.Map<CarDto>(car);

            return Ok(new ApiOkResponse(objcar));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateCarDto> Create([FromForm] CreateCarDto CarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var CarRequest = _mapper.Map<Car>(CarDto);

            // Create validator instance (or inject it)
            var CarValidator = new CarValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated

            var result = CarValidator.Validate(CarRequest);

            if (result.IsValid)
            {
                _unitOfWork.Cars.Create(CarRequest);

                return Ok(new ApiOkResponse(CarDto));
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);

            
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateCarDto> Update([FromForm] UpdateCarDto CarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var CarRequest = _mapper.Map<Car>(CarDto);

            var req = _unitOfWork.Cars.Update(CarRequest);


            return Ok(new ApiOkResponse(CarDto));
             
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<CarDto> Delete(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var car = _unitOfWork.Cars.Get(Id);

            if (car == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objcar = _mapper.Map<CarDto>(car);

            _unitOfWork.Cars.Delete(Id);

            return Ok(new ApiOkResponse(objcar));
        }

    }
}
