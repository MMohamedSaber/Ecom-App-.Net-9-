
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
    }
}
