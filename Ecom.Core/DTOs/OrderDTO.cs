
namespace Ecom.Core.DTOs
{
    public record OrderDTO
    {
     public int DeleveryMethodeId { get; set; }
        
        public string basketId {  get; set; }

        public ShipAddressDTO ShippAddress { get; set; }

    }


    public record ShipAddressDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
    }
}
