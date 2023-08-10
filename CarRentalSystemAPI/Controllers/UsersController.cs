using AutoMapper;
using CarRentalSystemAPI.Dtos;
using CarRentalSystemAPI.Response;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
		private readonly IMapper _mapper;

		private readonly IRepository<Users> _RepositoryUsers;

		private readonly IJWTManagerRepository _jWTManager;

		public UsersController(IJWTManagerRepository jWTManager, IRepository<Users> RepositoryUsers, IMapper mapper)
		{
			_jWTManager = jWTManager;
			_RepositoryUsers = RepositoryUsers;
			_mapper = mapper;
		}

        [HttpGet("getusers")]
        public ActionResult<UsersListDto> GetList([FromQuery] UsersRequestDto UsersDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var query = _RepositoryUsers.GetList(UsersDto.Search, UsersDto.Column, UsersDto.SortOrder, UsersDto.OrderBy, UsersDto.PageIndex, UsersDto.PageSize);

            var LstUsers= _mapper.Map<List<UsersDto>>(query.Items);

            UsersListDto UsrList = new UsersListDto();

            UsrList.Items = LstUsers;
            UsrList.TotalRows = query.TotalRows;
            UsrList.TotalPages = query.TotalPages;


            return Ok(new ApiOkResponse(UsrList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<UsersDto> Get(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var user = _RepositoryUsers.Get(Id);

            if (user == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objuser= _mapper.Map<UsersDto>(user);

            return Ok(new ApiOkResponse(objuser));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateUsersDto> Create([FromForm] CreateUsersDto UserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var UserRequest = _mapper.Map<Users>(UserDto);

            _RepositoryUsers.Create(UserRequest);

            return Ok(new ApiOkResponse(UserDto));
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateUsersDto> Update([FromForm] UpdateUsersDto UserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var UserRequest = _mapper.Map<Users>(UserDto);

            var req = _RepositoryUsers.Update(UserRequest);


            return Ok(new ApiOkResponse(UserDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<UsersDto> Delete(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var car = _RepositoryUsers.Get(Id);

            if (car == null)
            {
                return NotFound(new ApiResponse(404, $"Car not found with id {Id}"));
            }
            var objcar = _mapper.Map<CarDto>(car);

            _RepositoryUsers.Delete(Id);

            return Ok(new ApiOkResponse(objcar));
        }

        [HttpPost]
		[Route("authenticate")]
		public IActionResult Authenticate([FromForm] UsersDto UserDto)
		{
            var usersdata = _mapper.Map<Users>(UserDto);

            var token = _jWTManager.Authenticate(usersdata);

			if (token == null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}
	}

}
