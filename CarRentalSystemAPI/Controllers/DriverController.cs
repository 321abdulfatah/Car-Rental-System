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

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly IDriverService _driverService;
        public DriverController(IDriverService driverService, IMapper mapper)
        {
            _mapper = mapper;
            _driverService = driverService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] DriverRequestDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driverDetailsList = await _driverService.GetAllDriver();
            if (driverDetailsList == null)
            {
                return NotFound();
            }

            return Ok(new ApiOkResponse(driverDetailsList));
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var driver = await _driverService.GetDriverById(id);

            if (driver != null)
            {
                var driverDto = _mapper.Map<DriverDto>(driver);

                return Ok(new ApiOkResponse(driverDto));
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateDriverDto createDriverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driverDto = _mapper.Map<DriverDto>(createDriverDto);

            var driverValidator = new DriverValidator();


            var result = driverValidator.Validate(driverDto);

            if (result.IsValid)
            {

                var driverRequest = _mapper.Map<Driver>(driverDto);

                var isDriverCreated = await _driverService.CreateDriver(driverRequest);

                if (isDriverCreated)
                {
                    return Ok(new ApiOkResponse(driverDto));
                }
                else
                {
                    return BadRequest();
                }

            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateDriverDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driverRequest = _mapper.Map<Driver>(driverDto);

            var isDriverUpdated = await _driverService.UpdateDriver(driverRequest);
            if (isDriverUpdated)
            {
                return Ok(new ApiOkResponse(driverDto));
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var driver = _driverService.GetDriverById(id);

            var isDriverDeleted = await _driverService.DeleteDriver(id);

            if (isDriverDeleted)
            {
                var objDriver = _mapper.Map<DriverDto>(driver);

                return Ok(new ApiOkResponse(objDriver));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}