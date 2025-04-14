using Ecom.Core.DTOs;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("create-order")]
        public async Task<IActionResult> createOrder(OrderDTO orderDTO)
        {
            var Email =  User.FindFirst(ClaimTypes.Email)?.Value;
            var order=await _orderService.CreateOrderAsync(orderDTO, Email);    
            return Ok(order);
        }


        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> getOrderForUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var orders=await _orderService.GetAllOrdersForUserAscyn(email);
            return Ok(orders);
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> get(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var order =await _orderService.GetOrderByIdAsync(id,email);
            return Ok(order);
        }
    }
}
