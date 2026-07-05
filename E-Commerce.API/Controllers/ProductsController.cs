using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ApiBaseController
{
    //Get: api/Products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts(CancellationToken ct = default)
    {
        var result = await productService.GetAllProductsAsync(ct);

        return ToActionResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await productService.GetProductByIdAsync(id, ct);

        return ToActionResult(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct = default)
    {
        var result = await productService.GetAllTypesAsync(ct);

        return ToActionResult(result);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct = default)
    {
        var result = await productService.GetAllBrandsAsync(ct);

        return ToActionResult(result);
    }
}
