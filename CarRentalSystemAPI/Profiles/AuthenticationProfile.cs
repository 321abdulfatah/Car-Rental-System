using AutoMapper;
using DataAccessLayer.Models;
using CarRentalSystemAPI.Dtos;

namespace CarRentalSystemAPI.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginModelDto, LoginModel>().ReverseMap();

            CreateMap<RegistrationModelDto, RegistrationModel>().ReverseMap();

        }
    }
}
