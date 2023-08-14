using AutoMapper;
using CarRentalSystemAPI.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateCustomerDto, Customer>();

            // get , delete
            CreateMap<CustomerDto, Customer>().ReverseMap();

            

        }

    }

   
}
