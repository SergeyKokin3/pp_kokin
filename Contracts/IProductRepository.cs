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
        IEnumerable<Product> GetProducts(Guid shopId, bool trackChanges);
        Product GetProduct(Guid shopId, Guid id, bool trackChanges);
    }
}
