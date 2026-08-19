using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Contracts;

public interface IOrderService
{
    Task<Result<OrderToReturn>> CreateOrderAsync(OrderDto dto, string email, CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrderToReturn>>> GetOrdersForSpecificUserAsync(string email, CancellationToken ct = default);

    Task<Result<OrderToReturn>> GetOrderByIdForSpecificUserAsync(Guid orderId,string email, CancellationToken ct = default);

    Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default);
}
