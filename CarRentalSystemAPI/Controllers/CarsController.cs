using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;

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
        public ActionResult<CarListDto> GetList([FromQuery] CarRequestDto CarDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var query = _RepositoryCar.GetList(CarDto.Search, CarDto.Column, CarDto.SortOrder, CarDto.OrderBy, CarDto.PageIndex, CarDto.PageSize);

            var Lstcar = _mapper.Map<List<CarDto>>(query.Items);

            CarListDto carList =  new CarListDto();

            carList.Items = Lstcar;
            carList.TotalRows = query.TotalRows;
            carList.TotalPages = query.TotalPages;


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
            var car = _RepositoryCar.Get(Id);

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

            _RepositoryCar.Create(CarRequest);

            return Ok(new ApiOkResponse(CarDto));
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

            var req = _RepositoryCar.Update(CarRequest);


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

            var car = _RepositoryCar.Get(Id);

            if (car == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objcar = _mapper.Map<CarDto>(car);

            _RepositoryCar.Delete(Id);

            return Ok(new ApiOkResponse(objcar));
        }

    }
}
