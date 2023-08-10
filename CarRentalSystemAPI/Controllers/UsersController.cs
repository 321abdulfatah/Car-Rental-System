using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
	{
		private readonly IJWTManagerRepository _jWTManager;

		public UsersController(IJWTManagerRepository jWTManager)
		{
			this._jWTManager = jWTManager;
		}

		[HttpPost]
		[Route("authenticate")]
		public IActionResult Authenticate([FromQuery] Users usersdata)
		{
			var token = _jWTManager.Authenticate(usersdata);

			if (token == null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}
	}
}
