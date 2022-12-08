using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
            (
            new Product
            {
                Id = new Guid("90abbca8-664d-4b20-b5de-024705497d4a"),
                Name = "Moloko Prostokvashino",
                Quantity = 55,
                Price = 78.8,
                ShopId = new Guid("a9d4c053-49b6-410c-bc78-2d54a9991870")
            },
            new Product
            {
                Id = new Guid("96dba8c0-d178-41e7-938c-ed49778fb52a"),
                Name = "Chipsy Lays",
                Quantity = 200,
                Price = 99.99,
                ShopId = new Guid("a9d4c053-49b6-410c-bc78-2d54a9991870")
            },
            new Product
            {
                Id = new Guid("921ca3c1-0deb-4afd-ae94-2159a8479811"),
                Name = "Kolbasa Starodvorskaya",
                Quantity = 80,
                Price = 129.59,
                ShopId = new Guid("4d490a70-94ce-4d15-9494-5248280c2ce3")
            });
        }
    }
}
