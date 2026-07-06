using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications;

public class ProductSpecification : BaseSpecification<Product, int>
{
    public ProductSpecification(ProductQueryParams queryParams) : 
        base
        (
            P => 
            (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
            &&
            (!queryParams.TypeID.HasValue || P.TypeId == queryParams.TypeID)      
            &&
            (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue))
        )
    {
        AddInclude(P => P.Brand);
        AddInclude(P => P.Type);

        switch (queryParams.Sort)
        {
            case
                ProductSortOptions.NameAsc:
                AddOrderBy(P => P.Name);
                break;
            case
                ProductSortOptions.NameDesc:
                AddOrderByDesc(P => P.Name);
                break;
            case 
                ProductSortOptions.PriceAsc: 
                AddOrderBy(P => P.Price);
                break;
            case 
                ProductSortOptions.PriceDesc:
                AddOrderByDesc(P => P.Price);
                break;
            default:
                AddOrderBy(P => P.Id);
                break;
        }

        ApplyPagination(queryParams.PageIndex, queryParams.PageSize);
    }

    public ProductSpecification(int id) : base(P => P.Id == id)
    {
        AddInclude(P => P.Brand);
        AddInclude(P => P.Type);
    }
}
