using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Classes;

public class OrderService(
    IMapper mapper,
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork
    ) : IOrderService
{
    public async Task<Result<OrderToReturn>> CreateOrderAsync(OrderDto dto, string email, CancellationToken ct = default)
    {
        // Create Order

        // Address 
        var OrderAddress = mapper.Map<OrderAddress>(dto.ShipToAddress);

        // OrderItems 
        // Get Basket by Id 
        var basket = await basketRepository.GetBasketAsync(dto.BasketId, ct);

        if (basket is null)
        {
            return Result<OrderToReturn>.Fail(Error.NotFound("Basket.NotFound", $"Basket with Id {dto.BasketId} Not Found"));
        }

        if (basket.Items.Count == 0)
        {
            return Result<OrderToReturn>.Fail(Error.Validation("Basket is Empty", $"Can not create Order with Basket  Id {dto.BasketId}"));
        }

        var orderItems = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id, ct);

            if (product is null)
                return Result<OrderToReturn>.Fail(Error.NotFound("Product.NotFound", $"Product with Id {item.Id} Not Found"));

            var productItem = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

            if(product.Price != item.Price) item.Price = product.Price;

            var orderItem = new OrderItem(productItem, item.Price, item.Quantity);
            orderItems.Add(orderItem);
        }

        // Delivery Method
        var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(dto.DeliveryMethodId, ct);

        if (deliveryMethod is null)
        {
            return Result<OrderToReturn>.Fail(Error.NotFound("deliveryMethod.NotFound", $"deliveryMethod with Id {dto.DeliveryMethodId} Not Found"));
        }

        // SubTotal 
        var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);

        var order = new Order(email, OrderAddress, orderItems, deliveryMethod, subTotal);

        unitOfWork.GetRepository<Order, Guid>().Add(order);

        var count = await unitOfWork.SaveChangesAsync(ct);
        if (count <= 0)
            return Result<OrderToReturn>.Fail(Error.Failure("Create.Order.Failure", "Can not Create this Order"));

        await basketRepository.DeleteBasketAsync(dto.BasketId, ct);


        var orderToReturn = mapper.Map<OrderToReturn>(order);

        return Result<OrderToReturn>.Ok(orderToReturn);
    }

    public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
    {
        var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);

        if (deliveryMethods.Any())
        {
            return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
        }

        return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound("DeliveryMethods.NotFound", "Not Found Any Delivery Methods"));
    }

    public async Task<Result<OrderToReturn>> GetOrderByIdForSpecificUserAsync(Guid orderId, string email, CancellationToken ct = default)
    {
        var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(orderId, email), ct);

        if (order is null)
            return Result<OrderToReturn>.Fail(Error.NotFound("Order.NotFound", $"Order with id {orderId} is not found"));

        return Result<OrderToReturn>.Ok(mapper.Map<OrderToReturn>(order));
    }

    public async Task<Result<IReadOnlyList<OrderToReturn>>> GetOrdersForSpecificUserAsync(string email, CancellationToken ct = default)
    {
        var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email), ct);


        if (orders.Any())
        {
            return Result<IReadOnlyList<OrderToReturn>>.Ok(mapper.Map<IReadOnlyList<OrderToReturn>>(orders));
        }

        return Result<IReadOnlyList<OrderToReturn>>.Fail(Error.NotFound("Orders.NotFound", $"User with Email {email} do not have any orders"));
    }
}
