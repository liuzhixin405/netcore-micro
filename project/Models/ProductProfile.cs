using AutoMapper;
using project.Dtos;
using project.Utility.Helper;

namespace project.Models
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>().ForMember(e => e.CreateTime, opt => opt.MapFrom(src => TimestampHelper.ToUnixTimeMilliseconds(DateTime.UtcNow)));
           
        }
    }
}
