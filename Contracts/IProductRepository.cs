﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(Guid shopId, bool trackChanges);
        Task<Product> GetProductAsync(Guid shopId, Guid id, bool trackChanges);
        void CreateProductForShop(Guid shopId, Product product);
        void DeleteProduct(Product product);
    }
}
