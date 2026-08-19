using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders;

public class Order : BaseEntity<Guid>
{
    public Order()
    {
        
    }

    public Order(string userEmail, OrderAddress shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal)
    {
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
    }

    public string UserEmail { get; set; } = default!;
    public OrderAddress ShippingAddress { get; set; } = default!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public DeliveryMethod DeliveryMethod { get; set; } = default!;
    public decimal SubTotal { get; set; }

    public int DeliveryMethodId { get; set; }

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal GetTotal() => SubTotal + (DeliveryMethod?.Price ?? 0);
}
