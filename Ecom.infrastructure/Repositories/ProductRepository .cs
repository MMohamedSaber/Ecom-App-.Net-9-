
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
