using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers;

public class OrdersController(IOrderService orderService) : ApiBaseController
{
    // POST Create Order ==> Order (BasketId, DeliveryMethodId, ShipToAddress )
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderToReturn>> CreateOrderAsync(OrderDto dto, CancellationToken ct = default)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
        var result = await orderService.CreateOrderAsync(dto, email, ct);

        return ToActionResult(result);
    }

    // GET Orders Of User ==> [Email] --> current user orders for logged in user
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<OrderToReturn>>> GetOrders(CancellationToken ct = default)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

        var result = await orderService.GetOrdersForSpecificUserAsync(email, ct);

        return ToActionResult(result);
    }

    // GET Order By Id Of User ==> [Id + Email] --> current user order for logged in user
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<OrderToReturn>> GetOrderById(Guid id, CancellationToken ct = default)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

        var result = await orderService.GetOrderByIdForSpecificUserAsync(id, email, ct);

        return ToActionResult(result);
    }

    // GET Delivery Method ==> List Of Delivery Methods
    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct = default)
    {
        var result = await orderService.GetAllDeliveryMethodsAsync(ct);

        return ToActionResult(result);
    }
}
