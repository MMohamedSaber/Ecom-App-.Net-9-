namespace Ecom.Core.Entities.Orders
{
    public class DeliveryMethod:BaseEntity<int>
    {

        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string name, int price, string shippingTime, string description)
        {
            Name = name;
            Price = price;
            ShippingTime = shippingTime;
            Description = description;
        }

        public string Name{ get; set; }
        public int Price { get; set; }
        public string ShippingTime { get; set; }
        public string Description { get; set; }

    }
}