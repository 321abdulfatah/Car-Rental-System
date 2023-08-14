using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;


        public DriverController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("getdrivers")]
        public ActionResult<DriverListDto> GetList([FromQuery] DriverRequestDto DriverDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driverDetailsList = _unitOfWork.Drivers.GetList(DriverDto.Search, DriverDto.Column, DriverDto.SortOrder, DriverDto.OrderBy, DriverDto.PageIndex, DriverDto.PageSize);
            if (driverDetailsList == null)
            {
                return NotFound();
            }

            var Lstdriver = _mapper.Map<List<DriverDto>>(driverDetailsList.Items);

            DriverListDto driverList = new DriverListDto();

            driverList.Items = Lstdriver;
            driverList.TotalRows = driverDetailsList.TotalRows;
            driverList.TotalPages = driverDetailsList.TotalPages;


            return Ok(new ApiOkResponse(driverList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<DriverDto> Get(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driver= _unitOfWork.Drivers.Get(Id);

            if (driver == null)
            {
                return NotFound(new ApiResponse(404, $"Driver not found with id {Id}"));
            }
            var objdriver = _mapper.Map<DriverDto>(driver);

            return Ok(new ApiOkResponse(objdriver));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateDriverDto> Create([FromForm] CreateDriverDto DriverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var DriverRequest = _mapper.Map<Driver>(DriverDto);

            _unitOfWork.Drivers.Create(DriverRequest);

            return Ok(new ApiOkResponse(DriverDto));
           
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateDriverDto> Update([FromForm] UpdateDriverDto DriverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var DriverRequest = _mapper.Map<Driver>(DriverDto);

            _unitOfWork.Drivers.Update(DriverRequest);

            return Ok(new ApiOkResponse(DriverDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<DriverDto> Delete(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driver = _unitOfWork.Drivers.Get(Id);

            if (driver == null)
            {
                return NotFound(new ApiResponse(404, $"Driver not found with id {Id}"));
            }
            var objdriver = _mapper.Map<CarDto>(driver);

            _unitOfWork.Drivers.Delete(Id);

            return Ok(new ApiOkResponse(objdriver));
        }
    }
}