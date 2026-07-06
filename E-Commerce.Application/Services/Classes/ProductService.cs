using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Services.Classes;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
    {
        var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
        // mapping
        var brandsDTO = mapper.Map<IReadOnlyList<BrandDto>>(brands);

        return Result<IReadOnlyList<BrandDto>>.Ok(brandsDTO);
    }

    public async Task<Result<PaginationResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
    {

        var specs = new ProductSpecification(queryParams);
      
        var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs, ct);

        var productsDTO = mapper.Map<IReadOnlyList<ProductDto>>(products);

        var countSpecs = new ProductsCountSpecifications(queryParams);

        var count = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs, ct);

        var value = new PaginationResult<ProductDto>(queryParams.PageIndex, queryParams.PageSize, count, productsDTO);

        return Result<PaginationResult<ProductDto>>.Ok(value);
    }

    public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
    {
        var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);

        var typesDTO = mapper.Map<IReadOnlyList<TypeDto>>(Types);

        return Result<IReadOnlyList<TypeDto>>.Ok(typesDTO);
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
    {

        var specs = new ProductSpecification(id);

        var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(specs, id, ct);

        if (product is null)
        {
            return Result<ProductDto>.Fail(Error.NotFound("Product.NotFound", $"Product with id {id} is not found !"));
        }

        var productDTO = mapper.Map<ProductDto>(product);

        return Result<ProductDto>.Ok(productDTO);
    }
}
