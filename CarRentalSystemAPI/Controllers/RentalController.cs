using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalSystemAPI.Dtos;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRepository<Rental> _RepositoryRental;

        public RentalController(IRepository<Rental> RepositoryRental)
        {
            _RepositoryRental = RepositoryRental;
        }

        [HttpGet("getrentals")]
        public IActionResult GetRentals(int page, int pageSize)//localhost..../api/cars/getcars
        {
            List<RentalDto> LstRental= new List<RentalDto>();

            var result = _RepositoryRental.GetList(page, pageSize);

            foreach (var rental in result)
            {
                RentalDto objrental = new RentalDto();

                objrental.Id = rental.Id;
                objrental.CarId = rental.CarId;
                objrental.CustomerId = rental.CustomerId;
                objrental.DriverId = rental.DriverId;
                objrental.Rent = rental.Rent;
                objrental.StatusRent = rental.StatusRent;
                objrental.StartDateRent = rental.StartDateRent;
                objrental.RentTerm = rental.RentTerm;
                LstRental.Add(objrental);
            }
            return Ok(LstRental);
        }

        // GET: api/<RentalController>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {

            var rental = _RepositoryRental.Get(id);

            RentalDto objrental = new RentalDto();

            objrental.Id = rental.Id;
            objrental.CarId = rental.CarId;
            objrental.CustomerId = rental.CustomerId;
            objrental.DriverId = rental.DriverId;
            objrental.Rent = rental.Rent;
            objrental.StatusRent = rental.StatusRent;
            objrental.StartDateRent = rental.StartDateRent;
            objrental.RentTerm = rental.RentTerm;


            return Ok(objrental);
        }


        // POST api/<RentalController>
        [HttpPost]
        public IActionResult Create([FromForm] RentalDto RentalDto)
        {
            Rental RentalRequest = new Rental();

            RentalRequest.Id = RentalDto.Id;
            RentalRequest.CarId = RentalDto.CarId;
            RentalRequest.CustomerId = RentalDto.CustomerId;
            RentalRequest.DriverId = RentalDto.DriverId;
            RentalRequest.Rent = RentalDto.Rent;
            RentalRequest.StatusRent = RentalDto.StatusRent;
            RentalRequest.StartDateRent = RentalDto.StartDateRent;
            RentalRequest.RentTerm = RentalDto.RentTerm;


            _RepositoryRental.Create(RentalRequest);

            return Ok();
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] RentalDto RentalDto)
        {

            try
            {
                if (id != RentalDto.Id)
                    return BadRequest("ID mismatch");

                Rental RentalRequest = new Rental();

                RentalRequest.Id = RentalDto.Id;
                RentalRequest.CarId = RentalDto.CarId;
                RentalRequest.CustomerId = RentalDto.CustomerId;
                RentalRequest.DriverId = RentalDto.DriverId;
                RentalRequest.Rent = RentalDto.Rent;
                RentalRequest.StatusRent = RentalDto.StatusRent;
                RentalRequest.StartDateRent = RentalDto.StartDateRent;
                RentalRequest.RentTerm = RentalDto.RentTerm;


                var req = _RepositoryRental.Update(RentalRequest);

                if (req == null) return NotFound($"Rental with Id = {id} not found");

                return Ok(RentalRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        // DELETE api/<RentalController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _RepositoryRental.Delete(id);
            return Ok("Record Deleted Successfully");
        }
    }
}
