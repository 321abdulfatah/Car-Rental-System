using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<CreateRentalDto, Rental>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateRentalDto, Rental>();

            // get , delete
            CreateMap<RentalDto, Rental>().ReverseMap();


            CreateMap<CreateRentalDto, RentalDto>();

        }

    }

   
}
