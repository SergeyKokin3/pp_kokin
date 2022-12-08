using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public IEnumerable<Product> GetProducts(Guid shopId, bool trackChanges) =>
        FindByCondition(e => e.ShopId.Equals(shopId), trackChanges)
        .OrderBy(e => e.Name);

        public Product GetProduct(Guid shopId, Guid id, bool trackChanges) =>
        FindByCondition(e => e.ShopId.Equals(shopId) && e.Id.Equals(id),
        trackChanges).SingleOrDefault();
    }

}
