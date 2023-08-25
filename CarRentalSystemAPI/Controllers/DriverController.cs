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
    public class DriverController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly IDriverService _driverService;
        public DriverController(IDriverService driverService, IMapper mapper)
        {
            _mapper = mapper;
            _driverService = driverService;
        }
        
        [HttpGet("{id}")]
        public async Task<DriverDto> GetAsync(Guid id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);

            var driverDto = _mapper.Map<DriverDto>(driver);

            return driverDto;

        }

        [HttpGet]
        public async Task<DriverListDto> GetListDriversAsync([FromQuery] DriverRequestDto driverDto)
        {
            var pagedDrivers = await _driverService.GetListDriversAsync(
                driverDto.SearchTerm,
                driverDto.SortBy,
                driverDto.PageIndex,
                driverDto.PageSize
            );

            var driverDtos = _mapper.Map<List<DriverDto>>(pagedDrivers.Data);

            return new DriverListDto { Data = driverDtos, TotalCount = pagedDrivers.TotalCount };
        }

        [HttpPost]
        public async Task<CreateDriverDto> CreateAsync([FromForm] CreateDriverDto createDriverDto)
        {
            var driverRequest = _mapper.Map<Driver>(createDriverDto);

            var isDriverCreated = await _driverService.CreateDriverAsync(driverRequest);

            if (isDriverCreated)
            {
                return createDriverDto;
            }
            else
            {
                var errorMessage = "Failed to create the driver due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        [HttpPut("{id}")]
        public async Task<UpdateDriverDto> UpdateAsync(Guid id, [FromForm] UpdateDriverDto updateDriverDto)
        {
            var driverRequest = _mapper.Map<Driver>(updateDriverDto);

            var isDriverUpdated = await _driverService.UpdateDriverAsync(driverRequest);
            if (isDriverUpdated)
            {
                return updateDriverDto;
            }
            else
            {
                var errorMessage = "Failed to update the driver due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        [HttpDelete("{id}")]
        public async Task<DriverDto> DeleteAsync(Guid id)
        {
            var driver = _driverService.GetDriverByIdAsync(id);

            var isDriverDeleted = await _driverService.DeleteDriverAsync(id);

            if (isDriverDeleted)
            {
                var driverDto = _mapper.Map<DriverDto>(driver);

                return driverDto;
            }
            else
            {
                var errorMessage = "Driver with the specified ID can not delete.";
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}