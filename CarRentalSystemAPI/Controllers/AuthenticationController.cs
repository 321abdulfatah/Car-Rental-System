using AutoMapper;
using BusinessAccessLayer.Exceptions;
using BusinessAccessLayer.Services.Interfaces;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IMapper mapper, IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<LoginModel> Login([FromForm] LoginModelDto model)
        {
            try
            {
                var loginMoel = _mapper.Map<LoginModel>(model);

                loginMoel.Token = await _authService.Login(loginMoel);
                
                return loginMoel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CustomException(ex.Message);
            }
        }

        [HttpPost]
        [Route("registeration")]
        public async Task<RegistrationModel> Register([FromForm] RegistrationModelDto model)
        {
            try
            {
                var registrationModel = _mapper.Map<RegistrationModel>(model);


                bool isRegistered = await _authService.Registeration(registrationModel, UserRoles.Admin);
                
                if(isRegistered)
                    return registrationModel;
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