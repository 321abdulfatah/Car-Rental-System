using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<CreateUsersDto, Users>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateUsersDto, Users>();

            // get , delete
            CreateMap<UsersDto, Users>().ReverseMap();

            CreateMap<CreateUsersDto, UsersDto>();

        }

    }

   
}
