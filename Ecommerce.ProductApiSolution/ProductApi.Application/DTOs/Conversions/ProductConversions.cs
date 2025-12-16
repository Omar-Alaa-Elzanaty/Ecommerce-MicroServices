using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversions
    {
        public static Product ToEntity(ProductDto dto) =>
            new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

        public static (ProductDto?,IEnumerable<ProductDto>?) FromEntity(Product product,IEnumerable<Product>? products)
        {
            if(product is not null || products is null)
            {
                var singleProduct = new ProductDto(
                    product!.Id,
                    product.Name,
                    product.Price,
                    product.Quantity
                );

                return (singleProduct, null);
            }

            if(products is not null || product is null)
            {
                var _products = products!.Select(p =>

                    new ProductDto(
                        p.Id,
                        p.Name,
                        p.Price,
                        p.Quantity
                    )
                ).ToList();

                return (null, _products);
            }

            return (null, null);
        }
    }
}
