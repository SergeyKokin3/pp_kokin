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
        IEnumerable<Shop> GetAllShops(bool trackChanges);
        Shop GetShop(Guid shopId, bool trackChanges);
    }
}
