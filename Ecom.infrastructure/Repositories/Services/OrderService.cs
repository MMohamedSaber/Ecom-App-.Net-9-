
using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.infrastructure.Repositories.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
            private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderdto, string buyer)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(orderdto.basketId);
                List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in basket.basketItems)
            {

                var Product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(Product.Id, item.Image, Product.Name, item.Price, item.Quantity);
                orderItems.Add(orderItem);

            } 
            var deliveryMethode=await _context.DeliveryMethods.FirstOrDefaultAsync(d=>d.Id==orderdto.DeleveryMethodeId);

            var subTota=orderItems.Sum(o=>o.Price*o.Quantity);
            
            var shipp= _mapper.Map<ShippingAddress>(orderdto.ShippAddress);

            var order=new Orders(buyer, subTota,shipp,deliveryMethode,orderItems);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(orderdto.basketId);

            var result=_mapper.Map<OrderToReturnDTO>(order);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
        => await _context.DeliveryMethods.AsNoTracking().ToListAsync(); 


        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAscyn(string Email)
        {
            var orders=await _context.Orders.Where(o=>o.BuyerEmail==Email)
               .Include(inc => inc.deliveryMethod)
               .Include(inc => inc.orderItems)
               .ToListAsync();

            var result=_mapper.Map< IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string email)
        {
            var order = await _context.Orders
                 .Where(o => o.Id == Id && o.BuyerEmail == email)
                 .Include(inc => inc.deliveryMethod)
                 .Include(inc => inc.orderItems)
                 .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);


            return result;
        
        }
    }
}
