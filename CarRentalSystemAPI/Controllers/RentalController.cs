using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using System.Linq.Expressions;

namespace CarRentalSystemAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService, IMapper mapper)
        {
            _mapper = mapper;   

            _rentalService = rentalService;
        }

        [HttpGet("getAll")]
        public async Task<List<RentalDto>> GetAllRentalsAsync()
        {
            var rentalDetailsList = await _rentalService.GetAllRentalAsync();

            var rentalDtos = _mapper.Map<List<RentalDto>>(rentalDetailsList);

            return rentalDtos;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<RentalDto> GetAsync(Guid id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);

            if (rental == null)
            {
                var errorMessage = "Rental with the specified ID was not found.";
                return new RentalDto { ErrorMessage = errorMessage };
            }
            var rentalDto = _mapper.Map<RentalDto>(rental);

            return rentalDto;

        }

        [HttpGet]
        public async Task<RentalListDto> GetListCarsAsync([FromQuery] RentalRequestDto rentalDto)
        {
            bool foundColumn = true;

            Expression<Func<Rental, bool>> filter = rental => true; // Initialize the filter to return all records

            if (!string.IsNullOrEmpty(rentalDto.columnName) && !string.IsNullOrEmpty(rentalDto.searchTerm))
            {
                /*var propertyInfo = typeof(Rental).GetProperty(rentalDto.columnName);
                if (propertyInfo != null)
                {
                    filter = rental => propertyInfo.GetValue(rental).ToString().Contains(rentalDto.searchTerm);
                }*/
                switch (rentalDto.columnName)
                {
                    case "Rent":
                        filter = rental => rental.Rent.ToString().Equals(rentalDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "RentTerm":
                        filter = rental => rental.RentTerm.ToString().Equals(rentalDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "StartDateRent":
                        filter = rental => rental.StartDateRent.ToString().Equals(rentalDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "StatusRent":
                        filter = rental => rental.StatusRent.ToString().Equals(rentalDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    default:
                        foundColumn = !foundColumn;
                        filter = rental => false;
                        break;
                }
            }

            var pagedRentals = await _rentalService.GetListRentalsAsync(
                filter,
                rentalDto.sortBy,
                rentalDto.isAscending,
                rentalDto.PageIndex,
                rentalDto.PageSize
            );

            var rentalDtos = _mapper.Map<List<RentalDto>>(pagedRentals.Data);

            if (!foundColumn)
            {
                var errorMessage = $"Column Name with {rentalDto.columnName} was not found.";

                return new RentalListDto { Data = rentalDtos, TotalCount = pagedRentals.TotalCount, ErrorMessage = errorMessage };
            }
            return new RentalListDto { Data = rentalDtos, TotalCount = pagedRentals.TotalCount };
        }

        [HttpPost]
        public async Task<RentalDto> CreateAsync([FromForm] CreateRentalDto createRentalDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to create the rental due to a validation error." : errorMessage;

                return new RentalDto { ErrorMessage = errorMessage };
            }

            var rentalDto = _mapper.Map<RentalDto>(createRentalDto);

            var rentalRequest = _mapper.Map<Rental>(rentalDto);

            var isRentalCreated = await _rentalService.CreateRentalAsync(rentalRequest);

            if (isRentalCreated)
            {
                return rentalDto;
            }
            else
            {
                var errorMessage = "Failed to create the rental due to a validation error.";

                return new RentalDto { ErrorMessage = errorMessage };
            }

        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<RentalDto> UpdateAsync(Guid id, [FromForm] UpdateRentalDto updateRentalDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to update the rental due to a validation error." : errorMessage;

                return new RentalDto { ErrorMessage = errorMessage };
            }

            var existingRental = await _rentalService.GetRentalByIdAsync(id);

            if (existingRental == null)
            {
                var errorMessage = "Rental with the specified ID was not found.";

                return new RentalDto { ErrorMessage = errorMessage };
            }

            var rentalDto = _mapper.Map<RentalDto>(updateRentalDto);

            var rentalRequest = _mapper.Map<Rental>(rentalDto);

            var isRentalUpdated = await _rentalService.UpdateRentalAsync(rentalRequest);
            if (isRentalUpdated)
            {
                return rentalDto;
            }
            else
            {
                var errorMessage = "Failed to update the rental due to a validation error.";

                return new RentalDto { ErrorMessage = errorMessage };
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<RentalDto> DeleteAsync(Guid id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);

            var isRentalDeleted = await _rentalService.DeleteRentalAsync(id);

            if (isRentalDeleted)
            {
                var rentalDto = _mapper.Map<RentalDto>(rental);

                return rentalDto;
            }
            else
            {
                var errorMessage = "Rental with the specified ID can not delete.";
                return new RentalDto { ErrorMessage = errorMessage };
            }
        }
    }
}