using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;
using static Ecom.Core.DTOs.OrderToReturnDTO;

namespace Ecom.Api.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders,OrderToReturnDTO>()
                .ForMember(o=>o.deliveryMethod
                 ,src=>src.
                 MapFrom(o=>o.deliveryMethod.Name))
                .ReverseMap();
                
            
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
            CreateMap<ShipAddressDTO,ShippingAddress>().ReverseMap();
        }
    }
}
