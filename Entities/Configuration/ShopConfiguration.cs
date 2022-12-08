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
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasData
            (
            new Shop
            {
                Id = new Guid("a9d4c053-49b6-410c-bc78-2d54a9991870"),
                Name = "Magnit",
                Address = "Voroshilova, 2",
                Country = "Russia"
            },
            new Shop
            {
                Id = new Guid("4d490a70-94ce-4d15-9494-5248280c2ce3"),

                Name = "Pyaterochka",
                Address = "Bolsevistskaya, 68",
                Country = "Russia"
            }
            );
        }
    }
}
