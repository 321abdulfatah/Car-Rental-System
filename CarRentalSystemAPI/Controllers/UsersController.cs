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

        private readonly IUnitOfWork _unitOfWork;

        private readonly IJWTManagerRepository _jWTManager;

		public UsersController(IJWTManagerRepository jWTManager, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_jWTManager = jWTManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
		}

        [HttpGet]
        public ActionResult<UsersListDto> GetList([FromQuery] UsersRequestDto usersDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var query = _unitOfWork.Users.GetList(usersDto.Search, usersDto.Column, usersDto.SortOrder, usersDto.OrderBy, usersDto.PageIndex, usersDto.PageSize);

            var lstUsers= _mapper.Map<List<UsersDto>>(query.Items);

            UsersListDto usrList = new UsersListDto();

            usrList.Items = lstUsers;
            usrList.TotalRows = query.TotalRows;
            usrList.TotalPages = query.TotalPages;


            return Ok(new ApiOkResponse(usrList));
        }

        // GET: api/<CarsController>
        [HttpGet("{id}")]
        public ActionResult<UsersDto> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var user = _unitOfWork.Users.Get(id);

            var objUser= _mapper.Map<UsersDto>(user);

            return Ok(new ApiOkResponse(objUser));
        }


        // POST api/<CarsController>
        [HttpPost]
        public ActionResult<CreateUsersDto> Create([FromForm] CreateUsersDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }
            var userRequest = _mapper.Map<Users>(userDto);

            _unitOfWork.Users.Create(userRequest);

            return Ok(new ApiOkResponse(userDto));
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public ActionResult<UpdateUsersDto> Update([FromForm] UpdateUsersDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var userRequest = _mapper.Map<Users>(userDto);

            _unitOfWork.Users.Update(userRequest);

            return Ok(new ApiOkResponse(userDto));

        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public ActionResult<UsersDto> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState));
            }

            var user = _unitOfWork.Users.Get(id);

            var objUser = _mapper.Map<UsersDto>(user);

            _unitOfWork.Users.Delete(id);

            return Ok(new ApiOkResponse(objUser));
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
