﻿using AutoMapper;
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


        [HttpGet("getAll")]
        public async Task<List<DriverDto>> GetAllDriversAsync()
        {
            var driverDetailsList = await _driverService.GetAllDriverAsync();

            var driverDtos = _mapper.Map<List<DriverDto>>(driverDetailsList);

            return driverDtos;

        }

        
        [HttpGet("{id}")]
        public async Task<DriverDto> GetAsync(Guid id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);

            if (driver == null)
            {
                var errorMessage = "Driver with the specified ID was not found.";
                return new DriverDto { ErrorMessage = errorMessage };
            }
            var driverDto = _mapper.Map<DriverDto>(driver);

            return driverDto;

        }

        [HttpGet]
        public async Task<DriverListDto> GetListDriversAsync([FromQuery] DriverRequestDto driverDto)
        {
            bool foundColumn = true;

            Expression<Func<Driver, bool>> filter = driver => true; // Initialize the filter to return all records

            if (!string.IsNullOrEmpty(driverDto.columnName) && !string.IsNullOrEmpty(driverDto.searchTerm))
            {
                /*var propertyInfo = typeof(Driver).GetProperty(driverDto.columnName);
                if (propertyInfo != null)
                {
                    filter = driver => propertyInfo.GetValue(driver).ToString().Contains(driverDto.searchTerm);
                }*/
                switch (driverDto.columnName)
                {
                    case "Name":
                        filter = driver => driver.Name.Contains(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Address":
                        filter = driver => driver.Address.Contains(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Gender":
                        filter = driver => driver.Gender.Equals(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Age":
                        filter = driver => driver.Age.ToString().Equals(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Phone":
                        filter = driver => driver.Phone.ToString().Equals(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Email":
                        filter = driver => driver.Email.Equals(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Salary":
                        filter = driver => driver.Salary.ToString().Equals(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "IsAvailable":
                        bool searchValue = false; // Convert the search term to a boolean value
                        bool.TryParse(driverDto.searchTerm, out searchValue);
                        filter = driver => driver.IsAvailable == searchValue; // Apply the search filter if searchTerm is not null or empty
                        break;
                    default:
                        foundColumn = !foundColumn;
                        filter = driver => false;
                        break;
                }
            }

            else if (!string.IsNullOrWhiteSpace(driverDto.searchTerm))
            {
                filter = driver => driver.Name.Contains(driverDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
            }

            var pagedDrivers = await _driverService.GetListDriversAsync(
                filter,
                driverDto.sortBy,
                driverDto.isAscending,
                driverDto.PageIndex,
                driverDto.PageSize
            );

            var driverDtos = _mapper.Map<List<DriverDto>>(pagedDrivers.Data);

            if (!foundColumn)
            {
                var errorMessage = $"Column Name with {driverDto.columnName} was not found.";

                return new DriverListDto { Data = driverDtos, TotalCount = pagedDrivers.TotalCount, ErrorMessage = errorMessage };
            }
            return new DriverListDto { Data = driverDtos, TotalCount = pagedDrivers.TotalCount };
        }

        [HttpPost]
        public async Task<DriverDto> CreateAsync([FromForm] CreateDriverDto createDriverDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to create the drive due to a validation error." : errorMessage;

                return new DriverDto { ErrorMessage = errorMessage };
            }

            var driverDto = _mapper.Map<DriverDto>(createDriverDto);

            var driverRequest = _mapper.Map<Driver>(driverDto);

            var isDriverCreated = await _driverService.CreateDriverAsync(driverRequest);

            if (isDriverCreated)
            {
                return driverDto;
            }
            else
            {
                var errorMessage = "Failed to create the driver due to a validation error.";

                return new DriverDto { ErrorMessage = errorMessage };
            }
        }

        [HttpPut("{id}")]
        public async Task<DriverDto> UpdateAsync(Guid id, [FromForm] UpdateDriverDto updateDriverDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to update the driver due to a validation error." : errorMessage;

                return new DriverDto { ErrorMessage = errorMessage };
            }

            var existingDriver= await _driverService.GetDriverByIdAsync(id);

            if (existingDriver == null)
            {
                var errorMessage = "Driver with the specified ID was not found.";

                return new DriverDto { ErrorMessage = errorMessage };
            }

            var driverDto = _mapper.Map<DriverDto>(updateDriverDto);

            var driverRequest = _mapper.Map<Driver>(updateDriverDto);

            var isDriverUpdated = await _driverService.UpdateDriverAsync(driverRequest);
            if (isDriverUpdated)
            {
                return driverDto;
            }
            else
            {
                var errorMessage = "Failed to update the driver due to a validation error.";

                return new DriverDto { ErrorMessage = errorMessage };
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
                return new DriverDto { ErrorMessage = errorMessage };
            }
        }
    }
}