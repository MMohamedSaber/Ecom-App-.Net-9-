
namespace Ecom.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public ICustomerBasketRepository CustomerBasketRepository { get; }
        public IAuth AuthRepository { get; }

    }
}
