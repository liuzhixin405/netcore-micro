using AutoMapper;
using project.Dtos;
using project.Utility.Helper;

namespace project.Models
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>().ForMember(e => e.CreateTime, opt => opt.MapFrom(src => TimestampHelper.ToUnixTimeMilliseconds(DateTime.UtcNow)));
        }
    }
}
