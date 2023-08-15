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

        [HttpGet]
        public ActionResult<CarListDto> GetList([FromQuery] CarRequestDto carDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var carDetailsList = _unitOfWork.Cars.GetList(carDto.Search, carDto.Column, carDto.SortOrder, carDto.OrderBy, carDto.PageIndex, carDto.PageSize);
            if (carDetailsList == null)
            {
                return NotFound();
            }

            var lstCar =  _mapper.Map<List<CarDto>>(carDetailsList.Items);

            CarListDto carList =  new CarListDto();

            carList.Items = lstCar;
            carList.TotalRows = carDetailsList.TotalRows;
            carList.TotalPages = carDetailsList.TotalPages;


            return Ok(new ApiOkResponse(carList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<CarDto> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var car = _unitOfWork.Cars.Get(id);

           
            var objCar = _mapper.Map<CarDto>(car);

            return Ok(new ApiOkResponse(objCar));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CarDto> Create([FromForm] CreateCarDto createCarDto)
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

                _unitOfWork.Cars.Create(carRequest);

                return Ok(new ApiOkResponse(carDto));
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
                      
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateCarDto> Update([FromForm] UpdateCarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var carRequest = _mapper.Map<Car>(carDto);

            _unitOfWork.Cars.Update(carRequest);


            return Ok(new ApiOkResponse(carDto));
             
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<CarDto> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var car = _unitOfWork.Cars.Get(id);

         
            var objCar = _mapper.Map<CarDto>(car);

            _unitOfWork.Cars.Delete(id);

            return Ok(new ApiOkResponse(objCar));
        }

    }
}
