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
			public TrolleyCalculatorQuantity[] Quantities { get; set; }
			public double Total { get; set; }
		}

		public class TrolleyCalculatorQuantity
		{
			public TrolleyCalculatorQuantity() { }
			public TrolleyCalculatorQuantity(string name, int quantity) {
				Name = name;
				Quantity = quantity;
			}
			public string Name { get; set; }
			public int Quantity { get; set; }
		}
		public class TrolleyCalculatorRequestBody
		{
			public TrolleyCalculatorProduct[] Products { get; set; }
			public TrolleyCalculatorSpecial[] Specials { get; set; }
			public TrolleyCalculatorQuantity[] Quantities { get; set; }
		}

		internal decimal UseSpecial(IDictionary<string, int> quantities, IDictionary<string, double> regularPriceDic, TrolleyCalculatorSpecial[] specials, int from, decimal totalSoFar)
		{
			if (from > specials.Length) return totalSoFar;

			var originalQuantities = quantities.ToDictionary(x => x.Key, x => x.Value);
			if (from == specials.Length)
			{
				decimal total = 0;
				// Use retular price
				foreach (var kvp in quantities)
				{
					var name = kvp.Key;
					var quantity = kvp.Value;
					if (quantity > 0 && regularPriceDic.ContainsKey(name))
					{
						total += (decimal)regularPriceDic[name] * quantity;
					}
				}
				return total;
			}

			var special = specials[from];
			foreach(var s in special.Quantities)
			{
				if(quantities.ContainsKey(s.Name))
				{
					quantities[s.Name] -= s.Quantity;
				}
			}
			decimal totalIfUse = totalSoFar + (decimal)special.Total + UseSpecial(quantities, regularPriceDic, specials, from + 1, totalSoFar);

			decimal totalIfNotUse = totalSoFar + UseSpecial(originalQuantities, regularPriceDic, specials, from + 1, totalSoFar);

			return Math.Min(totalIfUse, totalIfNotUse);
		}

		[HttpPost]
		public ActionResult<decimal> Post([FromBody] TrolleyCalculatorRequestBody body)
		{
			var regularPriceDic = body.Products.ToDictionary(x => x.Name, x => x.Price);
			var quantities = body.Quantities.ToDictionary(x => x.Name, x => x.Quantity);
			var specials = body.Specials;

			var total = UseSpecial(quantities, regularPriceDic, specials, 0, 0);

			return total;
		}
	}
}
