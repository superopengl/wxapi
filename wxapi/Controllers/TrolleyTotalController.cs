using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace wxapi.Controllers
{
	
	[Route("api/trolleyTotal")]
	[ApiController]
	public class TrolleyTotalController : ControllerBase
	{
		public class TrolleyCalculatorProduct
		{
			public string Name { get; set; }
			public double Price { get; set; }
		}
		public class TrolleyCalculatorSpecial
		{
			public List<TrolleyCalculatorQuantity> Quantities { get; set; }
			public double Total { get; set; }
		}

		public class TrolleyCalculatorQuantity
		{
			public string Name { get; set; }
			public int Quantity { get; set; }
		}
		public class TrolleyCalculatorRequestBody
		{
			public List<TrolleyCalculatorProduct> Products { get; set; }
			public List<TrolleyCalculatorSpecial> Specials { get; set; }
			public List<TrolleyCalculatorQuantity> Quantities { get; set; }
		}

		internal bool CanUseSpecial(List<TrolleyCalculatorQuantity> quantities, TrolleyCalculatorSpecial special)
		{
			var specialDic = special.Quantities.ToDictionary(x => x.Name, x => x.Quantity);
			foreach (var q in quantities)
			{
				var name = q.Name;
				var quantity = q.Quantity;
				if (!specialDic.ContainsKey(name) || quantity < specialDic[name]) return false;
			}
			foreach(var q in quantities)
			{
				q.Quantity -= specialDic[q.Name];
			}
			return true;
		}

		[HttpPost]
		public ActionResult<decimal> Post([FromBody] TrolleyCalculatorRequestBody body)
		{
			var regularPriceDic = body.Products.ToDictionary(x => x.Name, x => x.Price);
			var quantities = body.Quantities;

			decimal total = 0;
			var specialList = body.Specials;
			for(var i = 0; i< specialList.Count; i++)
			{
				var special = specialList[i];
				if(CanUseSpecial(quantities, special))
				{
					total += (decimal)special.Total;
					i--; // To see if still can use this special.
				}
			}

			foreach(var q in quantities.Where(x => x.Quantity > 0))
			{
				if (!regularPriceDic.ContainsKey(q.Name))
				{
					throw new InvalidOperationException($"Cannot calculate for product '{q.Name}'. Probabaly because the price is't provided.");
				}

				total += (decimal)regularPriceDic[q.Name] * q.Quantity;
			}

			return total;
		}
	}
}
