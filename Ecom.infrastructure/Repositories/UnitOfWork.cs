
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            CategoryRepository = new CategoryRepository (context);
            ProductRepository  = new ProductRepository  (context);
            PhotoRepository    = new PhotoRepository    (context);

        }                        
    }
}
