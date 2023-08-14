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
    public class RentalController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;


        public RentalController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getrentals")]
        public ActionResult<RentalListDto> GetList([FromQuery] RentalRequestDto RentalDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rentalDetailsList = _unitOfWork.Rentals.GetList(RentalDto.Search, RentalDto.Column, RentalDto.SortOrder, RentalDto.OrderBy, RentalDto.PageIndex, RentalDto.PageSize);
            if (rentalDetailsList == null)
            {
                return NotFound();
            }

            var Lstrental = _mapper.Map<List<RentalDto>>(rentalDetailsList.Items);

            RentalListDto rentalList = new RentalListDto();

            rentalList.Items = Lstrental;
            rentalList.TotalRows = rentalDetailsList.TotalRows;
            rentalList.TotalPages = rentalDetailsList.TotalPages;


            return Ok(new ApiOkResponse(rentalList));
        }

        // GET: api/<RentalssController>
        [HttpGet("{id}")]
        public ActionResult<RentalDto> Get(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rental = _unitOfWork.Rentals.Get(Id);

            if (rental == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objrental = _mapper.Map<RentalDto>(rental);

            return Ok(new ApiOkResponse(objrental));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateRentalDto> Create([FromForm] CreateRentalDto RentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var RentalRequest = _mapper.Map<Rental>(RentalDto);

            _unitOfWork.Rentals.Create(RentalRequest);

            return Ok(new ApiOkResponse(RentalDto));
           
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateRentalDto> Update([FromForm] UpdateRentalDto RentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var RentalRequest = _mapper.Map<Rental>(RentalDto);

            _unitOfWork.Rentals.Update(RentalRequest);

            return Ok(new ApiOkResponse(RentalDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<RentalDto> Delete(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rental = _unitOfWork.Rentals.Get(Id);

            if (rental == null)
            {
                return NotFound(new ApiResponse(404, $"Rental not found with id {Id}"));
            }
            var objrental = _mapper.Map<RentalDto>(rental);

            _unitOfWork.Rentals.Delete(Id);

            return Ok(new ApiOkResponse(objrental));
        }
    }
}