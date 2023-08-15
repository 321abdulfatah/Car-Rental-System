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
        [HttpGet]
        public ActionResult<DriverListDto> GetList([FromQuery] DriverRequestDto driverDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driverDetailsList = _unitOfWork.Drivers.GetList(driverDto.Search, driverDto.Column, driverDto.SortOrder, driverDto.OrderBy, driverDto.PageIndex, driverDto.PageSize);
            if (driverDetailsList == null)
            {
                return NotFound();
            }

            var lstDriver = _mapper.Map<List<DriverDto>>(driverDetailsList.Items);

            DriverListDto driverList = new DriverListDto();

            driverList.Items = lstDriver;
            driverList.TotalRows = driverDetailsList.TotalRows;
            driverList.TotalPages = driverDetailsList.TotalPages;


            return Ok(new ApiOkResponse(driverList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<DriverDto> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driver= _unitOfWork.Drivers.Get(id);

            var objDriver = _mapper.Map<DriverDto>(driver);

            return Ok(new ApiOkResponse(objDriver));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateDriverDto> Create([FromForm] CreateDriverDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driverRequest = _mapper.Map<Driver>(driverDto);

            _unitOfWork.Drivers.Create(driverRequest);

            return Ok(new ApiOkResponse(driverDto));
           
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateDriverDto> Update([FromForm] UpdateDriverDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driverRequest = _mapper.Map<Driver>(driverDto);

            _unitOfWork.Drivers.Update(driverRequest);

            return Ok(new ApiOkResponse(driverDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<DriverDto> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driver = _unitOfWork.Drivers.Get(id);

            var objDriver = _mapper.Map<DriverDto>(driver);

            _unitOfWork.Drivers.Delete(id);

            return Ok(new ApiOkResponse(objDriver));
        }

    }
}