using AutoMapper;
using BusinessAccessLayer.Data.Validate;
using BusinessAccessLayer.Services.Interfaces;
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

        public readonly IUsersService _usersService;

        private readonly IJWTManagerRepository _jWTManager;

		public UsersController(IJWTManagerRepository jWTManager, IUsersService usersService, IMapper mapper)
		{
			_jWTManager = jWTManager;
            _usersService = usersService;
            _mapper = mapper;
		}

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UsersRequestDto usersDto)//localhost..../api/cars/getcars
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var carDetailsList = await _usersService.GetAllUsers();
            if (carDetailsList == null)
            {
                return NotFound();
            }

            return Ok(new ApiOkResponse(carDetailsList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var user = await _usersService.GetUsersById(id);

            if (user != null)
            {
                var userDto = _mapper.Map<UsersDto>(user);

                return Ok(new ApiOkResponse(userDto));
            }
            else
            {
                return BadRequest();
            }

        }


        // POST api/<CarsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUsersDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var userDto = _mapper.Map<UsersDto>(createUserDto);

            var userValidator = new UsersValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated

            var result = userValidator.Validate(userDto);

            if (result.IsValid)
            {

                var userRequest = _mapper.Map<Users>(userDto);

                var isUserCreated = await _usersService.CreateUsers(userRequest);

                if (isUserCreated)
                {
                    return Ok(new ApiOkResponse(userDto));
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
        public async Task<IActionResult> Update([FromForm] UpdateUsersDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var userRequest = _mapper.Map<Users>(userDto);

            var isUserUpdated = await _usersService.UpdateUsers(userRequest);
            if (isUserUpdated)
            {
                return Ok(new ApiOkResponse(userDto));
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

            var user = _usersService.GetUsersById(id);

            var isUserDeleted = await _usersService.DeleteUsers(id);

            if (isUserDeleted)
            {
                var objUser = _mapper.Map<UsersDto>(user);

                return Ok(new ApiOkResponse(objUser));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
		[Route("authenticate")]
		public IActionResult Authenticate([FromForm] UsersDto userDto)
		{
            var usersData = _mapper.Map<Users>(userDto);

            var token = _jWTManager.Authenticate(usersData);

			if (token == null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}
	}

}
