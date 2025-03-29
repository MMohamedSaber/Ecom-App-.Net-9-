
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;

namespace Ecom.infrastructure.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
