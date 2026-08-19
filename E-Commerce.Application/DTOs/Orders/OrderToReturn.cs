using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Orders;

public class OrderToReturn
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = default!;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderAddressDTO ShippingAddress { get; set; } = default!;
    public ICollection<OrderItemDto> OrderItems { get; set; } = [];
    public string DeliveryMethod { get; set; } = default!;
    public decimal DeliveryMethodCost { get; set; }
    public string Status { get; set; } = default!;
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }

}
