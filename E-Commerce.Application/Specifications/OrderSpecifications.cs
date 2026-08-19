using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications;

public class OrderSpecifications : BaseSpecification<Order, Guid>
{
    public OrderSpecifications(string email) : base(O => O.UserEmail == email)
    {
        AddInclude(O => O.DeliveryMethod);
        AddInclude(O => O.OrderItems);
        AddOrderBy(O => O.OrderDate);
    }
    public OrderSpecifications(Guid orderId, string email) : base(O => O.UserEmail == email && O.Id == orderId)
    {
        AddInclude(O => O.DeliveryMethod);
        AddInclude(O => O.OrderItems);
        AddOrderBy(O => O.OrderDate);
    }
}
