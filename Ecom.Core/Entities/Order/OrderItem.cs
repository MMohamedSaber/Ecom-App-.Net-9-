namespace Ecom.Core.Entities.Orders
{
    public class OrderItem:BaseEntity<int>
    {

        public OrderItem()
        {
            
        }
        public OrderItem(int productItemId, string mainImage, string productName, decimal price, int quantity)
        {
            ProductItemId = productItemId;
            MainImage = mainImage;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public int ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}