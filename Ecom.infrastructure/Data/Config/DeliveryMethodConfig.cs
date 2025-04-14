
using Ecom.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.infrastructure.Data.Config
{
    public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {

        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(o => o.Price).HasColumnType("decimal(18,2)");

            builder.HasData(new DeliveryMethod { Id = 1, ShippingTime = "within week", Description = "The best in products shipping", Name = "Aramex", Price = 20 },
                new DeliveryMethod { Id = 2, ShippingTime = "within Two dayes", Description = "Make your products safe", Name = "DHL", Price = 10 }
                );
        }
    
    
    }
}
