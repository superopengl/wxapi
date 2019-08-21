using System.Collections.Generic;
using System.Threading.Tasks;
using wxapi.Data;

namespace wxapi.Helpers
{
	public interface IProductSorter
	{
		Task<IEnumerable<Product>> Sort(Task<IEnumerable<Product>> sourceTask);
	}
}
