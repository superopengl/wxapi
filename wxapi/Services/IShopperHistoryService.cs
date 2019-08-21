using System.Collections.Generic;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Services
{
    public interface IShopperHistoryService
    {
        Task<IEnumerable<ShopperHistory>> GetShopperHistory();
    }
}
