using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product,ProductDTO>().ForMember
                (p=>p.CategoryName ,
      
                op=>op.MapFrom
                (src=>src.Category.Name)).ReverseMap();

            CreateMap<Photo, PhotoDTO>().ReverseMap();

            CreateMap<AddProductDto, Product>().ForMember(p=>p.Photos
            ,src=>src.Ignore()
            ).ReverseMap();
        }
    }
}
