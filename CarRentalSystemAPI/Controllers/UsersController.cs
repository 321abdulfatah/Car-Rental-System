using AutoMapper;
using BusinessAccessLayer.Services.Interfaces;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CarRentalSystemAPI.Controllers
{
    //[Authorize]
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

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<UsersDto> GetAsync(Guid id)
        {
            var user = await _usersService.GetUsersByIdAsync(id);

            var userDto = _mapper.Map<UsersDto>(user);

            return userDto;
        }

        [HttpGet]
        public async Task<UsersListDto> GetListUsersAsync([FromQuery] UsersRequestDto userDto)
        {

            var pagedUsers= await _usersService.GetListUsersAsync(
                userDto.SearchTerm,
                userDto.SortBy,
                userDto.PageIndex,
                userDto.PageSize
            );

            var userDtos = _mapper.Map<List<UsersDto>>(pagedUsers.Data);

            return new UsersListDto { Data = userDtos, TotalCount = pagedUsers.TotalCount };
        }

        [HttpPost]
        public async Task<CreateUsersDto> CreateAsync([FromForm] CreateUsersDto createUserDto)
        {
            var userRequest = _mapper.Map<Users>(createUserDto);

            var isUserCreated = await _usersService.CreateUsersAsync(userRequest);

            if (isUserCreated)
            {
                return createUserDto;
            }
            else
            {
                var errorMessage = "Failed to create the user due to a validation error.";
                throw new InvalidOperationException(errorMessage);

            }
        }

        [HttpPut("{id}")]
        public async Task<UpdateUsersDto> UpdateAsync(Guid id, [FromForm] UpdateUsersDto updateUserDto)
        {
            if (id != updateUserDto.Id)
            {
                var errorMessage = $"The User cannot be updated because the {id} does not match the Id after the update {updateUserDto.Id}";
                throw new InvalidOperationException(errorMessage);
            }
            var userRequest = _mapper.Map<Users>(updateUserDto);

            var isuserUpdated = await _usersService.UpdateUsersAsync(userRequest);
            if (isuserUpdated)
            {
                return updateUserDto;
            }
            else
            {
                var errorMessage = "Failed to update the user due to a validation error.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task<UsersDto> DeleteAsync(Guid id)
        {
            var user = _usersService.GetUsersByIdAsync(id);

            var isUserDeleted = await _usersService.DeleteUsersAsync(id);

            if (isUserDeleted)
            {
                var userDto = _mapper.Map<UsersDto>(user);

                return userDto;
            }
            else
            {
                var errorMessage = "User with the specified ID can not delete.";
                throw new InvalidOperationException(errorMessage);
            }
        }

        [AllowAnonymous]
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
