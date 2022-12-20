using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllShopsAsync(bool trackChanges);
        Task<Shop> GetShopAsync(Guid shopId, bool trackChanges);
        void CreateShop(Shop shop);
        Task<IEnumerable<Shop>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteShop(Shop shop);
    }
}
