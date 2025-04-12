
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Sharing;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDto productDto);
        Task<bool> UpdateAsync(UpdateProductDto productDto);
        Task DeleteAsync(Product product);

    }
}
