using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateCarDto, Car>();

            
            CreateMap<CarDto, Car>();

            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Driver,
                opt => opt.MapFrom(src => src.Driver));

            CreateMap<CreateCarDto, CarDto>();


        }

    }

   
}
