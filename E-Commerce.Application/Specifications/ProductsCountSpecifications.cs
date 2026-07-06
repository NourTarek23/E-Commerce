using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications;

public class ProductsCountSpecifications : BaseSpecification<Product, int>
{
    public ProductsCountSpecifications(ProductQueryParams queryParams) :
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
       

        
    }
}
