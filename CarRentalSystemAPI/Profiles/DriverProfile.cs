using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<CreateDriverDto, Driver>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateDriverDto, Driver>();

            // get , delete
            CreateMap<DriverDto, Driver>().ReverseMap();

            CreateMap<CreateDriverDto, DriverDto>();



        }

    }

   
}
