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

        [HttpGet]
        public ActionResult<RentalListDto> GetList([FromQuery] RentalRequestDto rentalDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rentalDetailsList = _unitOfWork.Rentals.GetList(rentalDto.Search, rentalDto.Column, rentalDto.SortOrder, rentalDto.OrderBy, rentalDto.PageIndex, rentalDto.PageSize);
            if (rentalDetailsList == null)
            {
                return NotFound();
            }

            var lstRental = _mapper.Map<List<RentalDto>>(rentalDetailsList.Items);

            RentalListDto rentalList = new RentalListDto();

            rentalList.Items = lstRental;
            rentalList.TotalRows = rentalDetailsList.TotalRows;
            rentalList.TotalPages = rentalDetailsList.TotalPages;


            return Ok(new ApiOkResponse(rentalList));
        }

        // GET: api/<RentalssController>
        [HttpGet("{id}")]
        public ActionResult<RentalDto> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rental = _unitOfWork.Rentals.Get(id);

            var objRental = _mapper.Map<RentalDto>(rental);

            return Ok(new ApiOkResponse(objRental));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateRentalDto> Create([FromForm] CreateRentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var rentalRequest = _mapper.Map<Rental>(rentalDto);

            _unitOfWork.Rentals.Create(rentalRequest);

            return Ok(new ApiOkResponse(rentalDto));
           
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateRentalDto> Update([FromForm] UpdateRentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rentalRequest = _mapper.Map<Rental>(rentalDto);

            _unitOfWork.Rentals.Update(rentalRequest);

            return Ok(new ApiOkResponse(rentalDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<RentalDto> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var rental = _unitOfWork.Rentals.Get(id);

            var objRental = _mapper.Map<RentalDto>(rental);

            _unitOfWork.Rentals.Delete(id);

            return Ok(new ApiOkResponse(objRental));
        }
    }
}