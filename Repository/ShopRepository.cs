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
    public class ShopRepository : RepositoryBase<Shop>, IShopRepository
    {
        public ShopRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Shop>> GetAllShopsAsync(bool trackChanges) =>
       FindAll(trackChanges)
       .OrderBy(c => c.Name)
       .ToList();

        public async Task<Shop> GetShopAsync(Guid shopId, bool trackChanges) => FindByCondition(c
=> c.Id.Equals(shopId), trackChanges).SingleOrDefault();

        public void CreateShop(Shop shop) => Create(shop);

        public async Task<IEnumerable<Shop>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
FindByCondition(x => ids.Contains(x.Id), trackChanges).ToList();

        public void DeleteShop(Shop shop)
        {
            Delete(shop);
        }
    }
}
