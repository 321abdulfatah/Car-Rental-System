using BusinessAccessLayer.Exceptions;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var message = await _authService.Login(model);
                
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("registeration")]
        public async Task<RegistrationModel> Register([FromForm] RegistrationModel model)
        {
            try
            {
                
                bool  isRegistered = await _authService.Registeration(model, UserRoles.User);
                
                if(isRegistered)
                    return model;
                else
                {
                    var errorMessage = "Failed to create the car due to a validation error.";
                    throw new CustomException(errorMessage);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CustomException(ex.Message);
            }
        }

    }
}