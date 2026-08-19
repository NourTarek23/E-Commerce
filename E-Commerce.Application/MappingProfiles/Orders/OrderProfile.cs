using AutoMapper;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.MappingProfiles.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderAddressDTO, OrderAddress>().ReverseMap();

        CreateMap<Order, OrderToReturn>()
            .ForMember(D => D.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(D => D.DeliveryMethodCost, opt => opt.MapFrom(s => s.DeliveryMethod.Price))
            .ForMember(D => D.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(D => D.Total, opt => opt.MapFrom(s => s.GetTotal()));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(D => D.ProductId, opt => opt.MapFrom(S => S.Product.ProductId))
            .ForMember(D => D.ProductName, opt => opt.MapFrom(S => S.Product.ProductName))
            .ForMember(D => D.PictureUrl, opt => opt.MapFrom(S => S.Product.PictureUrl));

        CreateMap<DeliveryMethod, DeliveryMethodDto>();
   
    }
}
