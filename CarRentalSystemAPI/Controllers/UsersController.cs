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

        [HttpGet("getAll")]
        public async Task<List<UsersDto>> GetAllUsersAsync()
        {
            var usersDetailsList = await _usersService.GetAllUsersAsync();

            var usersDtos = _mapper.Map<List<UsersDto>>(usersDetailsList);

            return usersDtos;
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public async Task<UsersDto> GetAsync(Guid id)
        {
            var user = await _usersService.GetUsersByIdAsync(id);

            if (user == null)
            {
                var errorMessage = "User with the specified ID was not found.";
                return new UsersDto { ErrorMessage = errorMessage };
            }
            var userDto = _mapper.Map<UsersDto>(user);

            return userDto;
        }

        [HttpGet]
        public async Task<UsersListDto> GetListCarsAsync([FromQuery] UsersRequestDto userDto)
        {
            bool foundColumn = true;

            Expression<Func<Users, bool>> filter = user => true; // Initialize the filter to return all records

            if (!string.IsNullOrEmpty(userDto.columnName) && !string.IsNullOrEmpty(userDto.searchTerm))
            {
                /*var propertyInfo = typeof(Users).GetProperty(userDto.columnName);
                if (propertyInfo != null)
                {
                    filter = user => propertyInfo.GetValue(user,null).ToString().Contains(userDto.searchTerm);
                }*/
                switch (userDto.columnName)
                {
                    case "Name":
                        filter = user => user.Name.Contains(userDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    case "Password":
                        filter = user => user.Password.Contains(userDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
                        break;
                    default:
                        foundColumn = !foundColumn;
                        filter = user => false;
                        break;
                }
            }

            else if (!string.IsNullOrWhiteSpace(userDto.searchTerm))
            {
                filter = user => user.Name.Contains(userDto.searchTerm); // Apply the search filter if searchTerm is not null or empty
            }

            var pagedUsers = await _usersService.GetListUsersAsync(
                filter,
                userDto.sortBy,
                userDto.isAscending,
                userDto.PageIndex,
                userDto.PageSize
            );

            var userDtos = _mapper.Map<List<UsersDto>>(pagedUsers.Data);

            if (!foundColumn)
            {
                var errorMessage = $"Column Name with {userDto.columnName} was not found.";

                return new UsersListDto { Data = userDtos, TotalCount = pagedUsers.TotalCount, ErrorMessage = errorMessage};
            }
            return new UsersListDto { Data = userDtos, TotalCount = pagedUsers.TotalCount };
        }

        [HttpPost]
        public async Task<UsersDto> CreateAsync([FromForm] CreateUsersDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to create the user due to a validation error." : errorMessage;

                return new UsersDto { ErrorMessage = errorMessage };
            }

            var userDto = _mapper.Map<UsersDto>(createUserDto);

            var userRequest = _mapper.Map<Users>(userDto);

            var isUserCreated = await _usersService.CreateUsersAsync(userRequest);

            if (isUserCreated)
            {
                return userDto;
            }
            else
            {
                var errorMessage = "Failed to create the user due to a validation error.";

                return new UsersDto { ErrorMessage = errorMessage };
            }
        }

        [HttpPut("{id}")]
        public async Task<UsersDto> UpdateAsync(Guid id, [FromForm] UpdateUsersDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToString();

                errorMessage = (errorMessage == null) ? "Failed to update the user due to a validation error." : errorMessage;

                return new UsersDto { ErrorMessage = errorMessage };
            }

            var existingUser= await _usersService.GetUsersByIdAsync(id);

            if (existingUser == null)
            {
                var errorMessage = "User with the specified ID was not found.";

                return new UsersDto { ErrorMessage = errorMessage };
            }

            var userDto = _mapper.Map<UsersDto>(updateUserDto);

            var userRequest = _mapper.Map<Users>(updateUserDto);

            var isuserUpdated = await _usersService.UpdateUsersAsync(userRequest);
            if (isuserUpdated)
            {
                return userDto;
            }
            else
            {
                var errorMessage = "Failed to update the user due to a validation error.";

                return new UsersDto { ErrorMessage = errorMessage };
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
                return new UsersDto { ErrorMessage = errorMessage };
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
