using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

public class BasketsController(IBasketService basketService) : ApiBaseController
{
    // GET    /api/Baskets?id={id}    # Get basket
    [HttpGet("{id}")]
    public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct = default)
    {
        var result = await basketService.GetBasketAsync(id, ct);

        return ToActionResult(result);
    }

    // POST   /api/Baskets            # Create/Update basket
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto dto, CancellationToken ct = default)
    {
        var result = await basketService.CreateOrUpdateBasketAsync(dto,null, ct);

        return ToActionResult(result);
    }

    // DELETE /api/Baskets/{id}       # Delete basket
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct = default)
    {
        var result = await basketService.DeleteBasketAsync(id, ct);

        return ToActionResult(result);
    }
}
