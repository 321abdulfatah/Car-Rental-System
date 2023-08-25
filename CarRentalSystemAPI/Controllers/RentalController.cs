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

        [HttpGet("{id}")]
        public async Task<RentalDto> GetAsync(Guid id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);

            var rentalDto = _mapper.Map<RentalDto>(rental);

            return rentalDto;

        }

        [HttpGet]
        public async Task<RentalListDto> GetListRentalsAsync([FromQuery] RentalRequestDto rentalDto)
        {
            var pagedRentals = await _rentalService.GetListRentalsAsync(
                rentalDto.SearchTerm,
                rentalDto.SortBy,
                rentalDto.PageIndex,
                rentalDto.PageSize
            );

            var rentalDtos = _mapper.Map<List<RentalDto>>(pagedRentals.Data);

            return new RentalListDto { Data = rentalDtos, TotalCount = pagedRentals.TotalCount };
        }

        [HttpPost]
        public async Task<CreateRentalDto> CreateAsync([FromForm] CreateRentalDto createRentalDto)
        {
            var rentalRequest = _mapper.Map<Rental>(createRentalDto);

            var isRentalCreated = await _rentalService.CreateRentalAsync(rentalRequest);

            if (isRentalCreated)
            {
                return createRentalDto;
            }
            else
            {
                var errorMessage = "Failed to create the rental due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }

        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<UpdateRentalDto> UpdateAsync(Guid id, [FromForm] UpdateRentalDto updateRentalDto)
        {
            var rentalRequest = _mapper.Map<Rental>(updateRentalDto);

            var isRentalUpdated = await _rentalService.UpdateRentalAsync(rentalRequest);
            if (isRentalUpdated)
            {
                return updateRentalDto;
            }
            else
            {
                var errorMessage = "Failed to update the rental due to a validation error.";
                throw new InvalidOperationException(errorMessage);
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
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}