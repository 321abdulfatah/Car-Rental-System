using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class RentalController : ControllerBase
    {
        private readonly IMapper _mapper;

        public readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService, IMapper mapper)
        {
            _mapper = mapper;   

            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] RentalRequestDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rentalDetailsList = await _rentalService.GetAllRental();
            if (rentalDetailsList == null)
            {
                return NotFound();
            }

            return Ok(new ApiOkResponse(rentalDetailsList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rental = await _rentalService.GetRentalById(id);

            if (rental != null)
            {
                var rentalDto = _mapper.Map<RentalDto>(rental);

                return Ok(new ApiOkResponse(rentalDto));
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateRentalDto createRentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rentalDto = _mapper.Map<RentalDto>(createRentalDto);

            var rentalValidator = new RentalValidator();

            var result = rentalValidator.Validate(rentalDto);

            if (result.IsValid)
            {

                var rentalRequest = _mapper.Map<Rental>(rentalDto);

                var isRentalCreated = await _rentalService.CreateRental(rentalRequest);

                if (isRentalCreated)
                {
                    return Ok(new ApiOkResponse(rentalDto));
                }
                else
                {
                    return BadRequest();
                }

            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateRentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rentalRequest = _mapper.Map<Rental>(rentalDto);

            var isRentalUpdated = await _rentalService.UpdateRental(rentalRequest);
            if (isRentalUpdated)
            {
                return Ok(new ApiOkResponse(rentalDto));
            }
            return BadRequest();
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rental = _rentalService.GetRentalById(id);

            var isRentalDeleted = await _rentalService.DeleteRental(id);

            if (isRentalDeleted)
            {
                var objRental = _mapper.Map<RentalDto>(rental);

                return Ok(new ApiOkResponse(objRental));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}