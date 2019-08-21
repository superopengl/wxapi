using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Helpers
{
    public class PriceLow2HighSorter : IProductSorter
    {
        public async Task<IEnumerable<Product>> Sort(Task<IEnumerable<Product>> sourceTask)
        {
            if (sourceTask == null) throw new ArgumentNullException(nameof(sourceTask));
            var result = await sourceTask;
            return result?.OrderBy(x => x.Price);
        }
    }
}
