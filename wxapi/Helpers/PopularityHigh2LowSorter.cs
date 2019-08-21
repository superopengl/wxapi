using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wxapi.Data;
using wxapi.Services;

namespace wxapi.Helpers
{
    public class PopularityHigh2LowSorter : IProductSorter
    {
        private readonly IShopperHistoryService shopperHistoryService;

        public PopularityHigh2LowSorter(IShopperHistoryService shopperHistoryService)
        {
            this.shopperHistoryService = shopperHistoryService;
        }

        public async Task<IEnumerable<Product>> Sort(Task<IEnumerable<Product>> sourceTask)
        {
            if (sourceTask == null) throw new ArgumentNullException(nameof(sourceTask));

            var shopperHistoryTask = shopperHistoryService.GetShopperHistory();
            await Task.WhenAll(sourceTask, shopperHistoryTask);

            var result = sourceTask.Result;
            var shopperHistories = shopperHistoryTask.Result;

            var productQuantityMap = shopperHistories?
                .SelectMany(x => x.Products)
                .GroupBy(x => x.Name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            return result?.OrderByDescending(x => productQuantityMap.ContainsKey(x.Name) ? productQuantityMap[x.Name] : -1);
        }
    }
}
