using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Common.Models;

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

            // get , delete
            CreateMap<CarDto, Car>();

            // getList
            CreateMap<Car, CarDto>();

        }

    }

   
}
