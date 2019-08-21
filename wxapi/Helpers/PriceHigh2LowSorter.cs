using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Helpers
{
    public class PriceHigh2LowSorter : IProductSorter
    {
        public async Task<IEnumerable<Product>> Sort(Task<IEnumerable<Product>> sourceTask)
        {
            if (sourceTask == null) throw new ArgumentNullException(nameof(sourceTask));
            var result = await sourceTask;
            return result?.OrderByDescending(x => x.Price);
        }
    }
}
