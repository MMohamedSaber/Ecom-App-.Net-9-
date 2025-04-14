
using Ecom.Core.Entities.Orders;

namespace Ecom.Core.DTOs
{
    public record OrderToReturnDTO
    {
        public  int Id { get; set; }    
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public ShippingAddress shippingAddress { get; set; }
        public string deliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDTO> orderItems { get; set; }
        public string status { get; set; }

        public record OrderItemDTO
        {
            public int ProductItemId { get; set; }
            public string MainImage { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }

        }
    }
}
