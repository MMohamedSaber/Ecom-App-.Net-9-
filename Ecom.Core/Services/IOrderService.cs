
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;

namespace Ecom.Core.Services
{
    public interface IOrderService
    {
        Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderdto, string buyer);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAscyn(string Email);

        Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string email);

        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();

    }
}
