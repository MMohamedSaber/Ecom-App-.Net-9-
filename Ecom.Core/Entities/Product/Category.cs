namespace Ecom.Core.Entities.Product
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products = new HashSet<Product>();

    }
}
